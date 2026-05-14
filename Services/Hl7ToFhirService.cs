using System.Text.Json;

namespace Hl7ToFhirDemo.Services
{
    public class Hl7ToFhirService
    {


        private readonly ILogger<Hl7ToFhirService> _logger;

        public Hl7ToFhirService(ILogger<Hl7ToFhirService> logger)
        {
            _logger = logger;
        }

        public object Convert(string hl7)
        {
            _logger.LogInformation("HL7 conversion started at {time}", DateTime.UtcNow);

            if (string.IsNullOrWhiteSpace(hl7))
            {
                return new
                {
                    success = false,
                    error = "HL7 message is empty"
                };
            }

            var lines = hl7
      .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var pidLine = lines.FirstOrDefault(l => l.StartsWith("PID"));

            if (pidLine == null)
            {
                _logger.LogWarning("PID segment missing");
                return new
                {
                    success = false,
                    error = "PID segment missing"
                };
            }

            var patientId = GetField(lines, "PID", 3)?.Split('^')[0];

            if (string.IsNullOrWhiteSpace(patientId))
            {
                return new
                {
                    success = false,
                    error = "Patient ID missing"
                };
            }

            // Patient
            var patient = new
            {
                resourceType = "Patient",
                id = patientId,
                active = true,

                identifier = new[]
                {
                    new {
                        system = "http://hospital.smarthealth.org/mrn",
                        value = patientId
                    }
                },

                meta = new
                {
                    lastUpdated = DateTime.UtcNow.ToString("o")
                },

                name = new[]
                {
                    new {
                        family = GetNamePart(lines, 0),
                        given = new[] { GetNamePart(lines, 1) }
                    }
                },

                gender = MapGender(GetField(lines, "PID", 8)),
                birthDate = FormatDate(GetField(lines, "PID", 7))


            };

            // OBX → Observations


            var observations = lines
            .Where(l => l.StartsWith("OBX"))
            .Select(line =>
            {
                var parts = line.Split('|');

                var hasValue = double.TryParse(parts.Length > 5 ? parts[5] : "", out var val);

                var codeParts = parts.Length > 3 ? parts[3].Split('^') : new string[] { "", "" };

                var codeVal = codeParts.Length > 0 ? codeParts[0] : "";
                var displayVal = codeParts.Length > 1 ? codeParts[1] : codeVal;

                return new
                {
                    resourceType = "Observation",

                    meta = new
                    {
                        lastUpdated = DateTime.UtcNow.ToString("o")
                    },

                    status = "final",

                    effectiveDateTime = parts.Length > 14 && !string.IsNullOrEmpty(parts[14])
                        ? DateTime.ParseExact(parts[14], "yyyyMMddHHmmss", null).ToString("o")
                        : DateTime.UtcNow.ToString("o"),

                    code = new
                    {
                        coding = new[]
                        {
                            new {
                                system = "http://loinc.org",
                                code = codeVal,
                                display = displayVal
                            }
                        },
                        text = displayVal
                    },

                    subject = new
                    {
                        reference = $"Patient/{patientId}",
                        display = $"{GetNamePart(lines, 1)} {GetNamePart(lines, 0)}"
                    },

                    valueQuantity = hasValue ? new
                    {
                        value = val,
                        unit = parts.Length > 6 ? parts[6] : "",
                        system = "http://unitsofmeasure.org",
                        code = parts.Length > 6 ? parts[6] : ""
                    } : null,

                    category = new[]
                    {
                        new {
                            coding = new[]
                            {
                                new {
                                    system = "http://terminology.hl7.org/CodeSystem/observation-category",
                                    code = "laboratory",
                                    display = "Laboratory"
                                }
                            }
                        }
                    },

                    performer = new[]
                    {
                        new {
                            display = "Lab System"
                        }
                    }
                };
            }).ToList();

            var entries = new List<object>();

            var bundle = new
            {
                resourceType = "Bundle",
                type = "collection",
                entry = entries
            };

            entries.Add(new
            {
                fullUrl = $"urn:uuid:{Guid.NewGuid()}",
                resource = patient
            });


            foreach (var obs in observations)
            {
                var obsId = Guid.NewGuid().ToString();

                entries.Add(new
                {
                    fullUrl = $"urn:uuid:{obsId}",
                    resource = new
                    {
                        obs.resourceType,
                        id = obsId,
                        obs.meta,
                        obs.status,
                        obs.effectiveDateTime,
                        obs.code,
                        obs.subject,
                        obs.valueQuantity,
                        obs.category,
                        obs.performer
                    },
                    //request = new
                    //{
                    //    method = "POST",
                    //    url = "Observation"
                    //}

                });
            }



            var pv1Line = lines.FirstOrDefault(l => l.StartsWith("PV1"));

            if (pv1Line != null)
            {
                var parts = pv1Line.Split('|');

                var encounterId = Guid.NewGuid().ToString();

                var doctorParts = parts.Length > 7
                    ? parts[7].Split('^')
                    : new string[] { "", "", "" };

                var encounter = new
                {
                    resourceType = "Encounter",

                    id = encounterId,

                    status = "finished",

                    @class = new
                    {
                        system = "http://terminology.hl7.org/CodeSystem/v3-ActCode",

                        code = parts.Length > 2 && parts[2] == "I"
                            ? "IMP"
                            : "AMB",

                        display = parts.Length > 2 && parts[2] == "I"
                            ? "inpatient encounter"
                            : "ambulatory"
                    },

                    subject = new
                    {
                        reference = $"Patient/{patientId}",
                        display = $"{GetNamePart(lines, 1)} {GetNamePart(lines, 0)}"
                    },

                    location = new[]
                    {
            new
            {
                location = new
                {
                    display = parts.Length > 3
                        ? parts[3].Replace("^", " ")
                        : "Unknown"
                }
            }
        },

                    participant = new[]
                    {
            new
            {
                individual = new
                {
                    display = doctorParts.Length > 2
                        ? $"{doctorParts[2]} {doctorParts[1]}"
                        : "Unknown Doctor"
                }
            }
        }
                };

                entries.Add(new
                {
                    fullUrl = $"urn:uuid:{encounterId}",
                    resource = encounter
                });
            }

            _logger.LogInformation("FHIR bundle generated successfully");

            return bundle;
        }

        private string? GetField(string[] lines, string segment, int index)
        {
            var line = lines.FirstOrDefault(l => l.StartsWith(segment));
            if (line == null) return null;

            var parts = line.Split('|');
            return parts.Length > index ? parts[index] : null;
        }

        private string GetNamePart(string[] lines, int index)
        {
            var nameField = GetField(lines, "PID", 5);
            var parts = nameField?.Split('^');

            return parts != null && parts.Length > index ? parts[index] : "";
        }

        private string MapGender(string g) =>
            g switch
            {
                "M" => "male",
                "F" => "female",
                _ => "unknown"
            };

        private string? FormatDate(string hl7Date)
        {
            if (string.IsNullOrEmpty(hl7Date)) return null;
            return DateTime.ParseExact(hl7Date, "yyyyMMdd", null)
                           .ToString("yyyy-MM-dd");
        }
    }

}
using System.Collections.Concurrent;

namespace Hl7ToFhirDemo.Mappings
{
    public static class PractitionerMapper
    {
        public static object Map(string[] doctorParts)
        {
            if (doctorParts == null ||
                doctorParts.Length == 0 ||
                string.IsNullOrWhiteSpace(doctorParts[0]))
            {
                return null;
            }

            return new
            {
                resourceType = "Practitioner",

                id = doctorParts[0],

                identifier = new[]
                {
                    new
                    {
                        system = "http://hospital.smarthealth.org/provider",
                        value = doctorParts[0]
                    }
                },

                name = new[]
                {
                    new
                    {
                        family = doctorParts.Length > 2 ? doctorParts[2] : "",
                        given = new[]
                        {
                            doctorParts.Length > 1 ? doctorParts[1] : ""
                        }
                    }
                }
            };
        }
    }
}
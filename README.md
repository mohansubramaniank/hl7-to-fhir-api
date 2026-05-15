# HL7 to FHIR Conversion API

ASP.NET Core based healthcare interoperability API that converts HL7 v2 messages into FHIR resources such as Patient, Observation, and Encounter.

---

# Features

* HL7 v2 message parsing
* FHIR Bundle generation
* Patient resource mapping
* Observation (OBX) mapping
* Encounter (PV1) mapping
* HL7 file upload support
* Swagger API documentation
* Validation and error handling
* Logging support
* Docker deployment support
* Cloud deployment ready

---

# Technologies Used

* ASP.NET Core Web API
* C#
* Swagger / OpenAPI
* Docker
* Render Cloud Hosting
* GitHub

---

# Supported HL7 Segments

| HL7 Segment | FHIR Resource |
| ----------- | ------------- |
| PID         | Patient       |
| OBX         | Observation   |
| PV1         | Encounter     |

---

# API Endpoints

## Convert HL7 Message

### POST

```http
/api/hl7/convert
```

### Request

```json
{
  "hl7Message": "MSH|^~\\&|HospitalSystem|MainHospital|FHIRApp|IntegrationEngine|20260430120000||ADT^A01|MSG00001|P|2.3\rPID|1||67890^^^HospitalMRN||Smith^Alice||19920515|F\rPV1|1|I|Ward1^Room12^Bed5||||1234^Johnson^Robert\rOBX|1|NM|GLU^Glucose||105|mg/dL"
}
```

---

## Upload HL7 File

### POST

```http
/api/hl7/upload
```

Upload:

* `.hl7`
* `.txt`

---

# Sample FHIR Response

```json
{
  "resourceType": "Bundle",
  "type": "collection",
  "entry": [
    {
      "resource": {
        "resourceType": "Patient"
      }
    },
    {
      "resource": {
        "resourceType": "Observation"
      }
    },
    {
      "resource": {
        "resourceType": "Encounter"
      }
    }
  ]
}
```

---

# Validation Support

The API validates:

* Empty HL7 messages
* Missing PID segments
* Invalid OBX structures
* Missing patient identifiers

---

# Logging

Application logging added using ASP.NET Core ILogger.

Tracks:

* HL7 conversion requests
* Validation failures
* Successful FHIR bundle generation

---

# Deployment

Deployed using:

* Docker
* Render Cloud Platform

---

# Swagger API Documentation

Swagger UI available at:

```http
/swagger
```

---

# Future Enhancements

* Practitioner resource mapping
* AllergyIntolerance support
* MedicationRequest mapping
* Authentication & API keys
* Batch HL7 processing
* Azure/AWS deployment
* Database persistence

---

# Project Goal

This project demonstrates real-world healthcare interoperability workflows between legacy HL7 v2 systems and modern FHIR APIs.

---

# Author

Mohan Subramanian

Healthcare Integration | HL7 | FHIR | ASP.NET Core

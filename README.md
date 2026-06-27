![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-blue)
![FHIR](https://img.shields.io/badge/FHIR-R4-green)
![HL7](https://img.shields.io/badge/HL7-v2-orange)
![Docker](https://img.shields.io/badge/Docker-Ready-blue)
![License](https://img.shields.io/badge/License-MIT-green)

## Table of Contents

- Overview
- Why This Project
- Features
- Use Cases
- Architecture
- Technologies
- Supported HL7 Segments
- Supported Message Types
- API Endpoints
- Upload API
- Sample Response
- Validation
- Logging
- Deployment
- Swagger
- Roadmap
- Skills Demonstrated
- Vision
- Project Goal
- Author

# HL7 to FHIR Conversion API

Healthcare organizations still rely heavily on HL7 v2 messages while modern applications increasingly adopt the FHIR standard. This project bridges that gap by providing a lightweight ASP.NET Core API that parses HL7 v2 messages and converts them into FHIR resources such as Patient, Observation, and Encounter. It is designed for developers, integration engineers, and healthcare interoperability projects.
---
# Why This Project?

Many hospitals continue to exchange clinical information using HL7 v2 messages, while modern healthcare applications and APIs increasingly rely on the FHIR standard.

This project simplifies the transition by providing an API that converts HL7 messages into standardized FHIR resources, making integration with modern healthcare platforms easier.

# [Features](#features)

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
* Containerized using Docker and deployable to cloud platforms
  such as Render, Azure App Service, AWS ECS, or Kubernetes.

---

# [Architecture Overview](#architecture-overview)

![Architecture Diagram](hl7-to-fhir-diagram-1.png)

---

# Technologies Used

* ASP.NET Core 10.0
* C#
* HL7 v2.5.1
* FHIR R4
* Swagger / OpenAPI
* Docker
* Render Cloud Hosting
* GitHub

---
# Use Cases

- Hospital Information Systems
- Electronic Health Records (EHR)
- Laboratory Information Systems
- Healthcare Integration Engines
- HL7 to FHIR Migration Projects
- Healthcare API Development


# Supported HL7 Segments

| HL7 Segment | FHIR Resource |
| ----------- | ------------- |
| PID         | Patient       |
| OBX         | Observation   |
| PV1         | Encounter     |

---
## Supported HL7 Message Types

Current support:

- ADT^A01
- ADT^A04
- ORU^R01 (planned)

# [API Endpoints](#api-endpoints)

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


## Skills Demonstrated

Deployment

↓

Swagger

↓

Roadmap

↓

Skills

↓

Vision

↓

Author

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
        "resourceType": "Patient",
        "id": "67890",
        "name": [
          {
            "family": "Smith",
            "given": [
              "Alice"
            ]
          }
        ],
        "gender": "female",
        "birthDate": "1992-05-15"
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
## Roadmap

### Version 1.0

- [x] HL7 Parser
- [x] Patient Mapping
- [x] Observation Mapping
- [x] Encounter Mapping

### Version 1.1

- [ ] Practitioner
- [ ] Organization

### Version 1.2

- [ ] MedicationRequest
- [ ] AllergyIntolerance

### Version 2.0

- [ ] Web Dashboard
- [ ] AI Explanation
- [ ] Mapping Generator
---

# Swagger API Documentation

Swagger UI available at:

```http
/swagger
```
## Demo

- Video Demonstration
- Swagger UI
- Sample HL7 Message
  
# Vision

- This project is evolving into an open-source healthcare interoperability toolkit supporting HL7 parsing, FHIR conversion, validation, mapping, and AI-assisted healthcare integration.

# Planned Features

* Practitioner resource mapping
* AllergyIntolerance support
* MedicationRequest mapping
* Authentication & API keys
* Batch HL7 processing
* Azure/AWS deployment
* Database persistence

# Project Goal

This project demonstrates real-world healthcare interoperability workflows between legacy HL7 v2 systems and modern FHIR APIs.

# Author

Mohan Subramanian

Healthcare Integration Engineer

Specializing in

• HL7 v2
• FHIR R4
• ASP.NET Core
• Healthcare APIs
• System Integration

> **Note**
>
> This project is intended as a reference implementation for HL7 v2 to FHIR conversion and can be extended to support additional message types and FHIR resources.


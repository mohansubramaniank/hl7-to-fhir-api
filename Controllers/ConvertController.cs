using Hl7ToFhirDemo.Models;
using Hl7ToFhirDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hl7ToFhirDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConvertController : ControllerBase
    {

        private readonly Hl7ToFhirService _fhirservice;

        public ConvertController(Hl7ToFhirService fhirservice)
        {
            _fhirservice = fhirservice;
        }

        [HttpPost]
        public IActionResult ConvertHl7([FromBody] Hl7Request request)
        {
            if (string.IsNullOrEmpty(request.Hl7Message))
                return BadRequest("HL7 message is required");

            var result = _fhirservice.Convert(request.Hl7Message);
            //return Ok(result);
            return Ok(result);
        }
    }
}

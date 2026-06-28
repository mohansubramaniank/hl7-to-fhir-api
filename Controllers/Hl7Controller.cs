using Hl7ToFhirDemo.Models;
using Hl7ToFhirDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hl7ToFhirDemo.Controllers
{
    [ApiController]
    [Route("api/hl7")]
    public class Hl7Controller : ControllerBase
    {
        private readonly Hl7ToFhirService _fhirService;

        public Hl7Controller(Hl7ToFhirService fhirService)
        {
            _fhirService = fhirService;
        }

        [HttpPost("convert")]
        public IActionResult ConvertHl7([FromBody] Hl7Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Hl7Message))
            {
                return BadRequest("HL7 message is required.");
            }

            var result = _fhirService.Convert(request.Hl7Message);

            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadHl7File(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("HL7 file is required.");
            }

            string hl7Content;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                hl7Content = await reader.ReadToEndAsync();
            }

            var result = _fhirService.Convert(hl7Content);

            return Ok(result);
        }
    }
}

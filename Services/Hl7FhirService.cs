namespace Hl7ToFhirDemo.Services
{
    public class Hl7FhirService
    {
        public object ConvertToFhir(string hl7Message)
        {
            var lines = hl7Message
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Your existing Patient / Observation / Encounter logic here

            //return new
            //{
            //    message = "FHIR conversion completed"
            //};

            var entries = new List<object>();

            var bundle = new
            {
                resourceType = "Bundle",
                type = "collection",
                entry = entries
            };

            // Add Patient
            // Add Encounter
            // Add Observations

            return bundle;

        }
    }
}

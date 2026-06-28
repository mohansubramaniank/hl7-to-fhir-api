using Hl7ToFhirDemo.Middleware;
using Hl7ToFhirDemo.Services;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HL7 to FHIR API",
        Version = "v1",
        Description = "HL7 v2.x to FHIR R4 Conversion API"
    });
});

builder.Services.AddScoped<Hl7ToFhirService>();

#endregion

var app = builder.Build();

#region Middleware

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HL7 to FHIR API v1");
    c.RoutePrefix = "swagger";
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ApiKeyMiddleware>();

#endregion

#region Endpoints

app.MapControllers();
app.MapRazorPages();

#endregion

app.Run();
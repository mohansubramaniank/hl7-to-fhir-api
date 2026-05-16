using Hl7ToFhirDemo.Middleware;
using Hl7ToFhirDemo.Services;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HL7 to FHIR API",
        Version = "v1"
    });

    c.AddSecurityDefinition("x-api-key", new OpenApiSecurityScheme
    {
        Description = "API Key Authentication",
        Type = SecuritySchemeType.ApiKey,
        Name = "x-api-key",
        In = ParameterLocation.Header
    });
});

builder.Services.AddScoped<Hl7ToFhirService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapRazorPages();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();

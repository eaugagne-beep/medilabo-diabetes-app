using System.Net.Http.Headers;
using System.Text;
using AssessmentService.API.Security;
using AssessmentService.Application.Interfaces;
using AssessmentService.Application.Services;
using AssessmentService.Infrastructure.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuration de Swagger avec la sécurité Basic Authentication
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AssessmentService API",
        Version = "v1"
    });

    var basicSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Entrez votre username et password"
    };

    options.AddSecurityDefinition("Basic", basicSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services
    .AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
        "BasicAuthentication", null);

builder.Services.AddAuthorization();

var basicAuthValue = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:admin123"));

// Configuration des clients HTTP pour les services PatientService et NoteService
builder.Services.AddHttpClient<IPatientServiceClient, PatientServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://patientservice-api:8080");
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", basicAuthValue);
});

// Configuration du client HTTP pour le service NoteService
builder.Services.AddHttpClient<INoteServiceClient, NoteServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://noteservice-api:8080");
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", basicAuthValue);
});

builder.Services.AddScoped<IAssessmentService, AssessmentAppService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
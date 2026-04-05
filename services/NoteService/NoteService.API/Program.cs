using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using NoteService.API.Security;
using NoteService.Application.Interfaces;
using NoteService.Application.Services;
using NoteService.Infrastructure.Repositories;
using NoteService.Infrastructure.Services;
using NoteService.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuration de Swagger avec la sécurité Basic Authentication
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NoteService API",
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

    // Ajout de la définition de sécurité Basic Authentication à Swagger
    options.AddSecurityDefinition("Basic", basicSecurityScheme);

    // Ajout de l'exigence de sécurité pour toutes les opérations de l'API
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

// Configuration des paramètres de MongoDB à partir du fichier appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));


builder.Services.AddSingleton<NoteMongoService>();

// Configuration de l'authentification Basic Authentication
builder.Services
    .AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
        "BasicAuthentication", null);

// Configuration de l'autorisation
builder.Services.AddAuthorization();

// Configuration de l'injection de dépendances pour les services et les repositories
builder.Services.AddScoped<IPatientNoteRepository, PatientNoteRepository>();
builder.Services.AddScoped<IPatientNoteService, PatientNoteAppService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
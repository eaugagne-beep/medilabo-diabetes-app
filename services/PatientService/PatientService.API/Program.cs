using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PatientService.API.Security;
using PatientService.Application.Interfaces;
using PatientService.Application.Services;
using PatientService.Infrastructure.Data;
using PatientService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuration de Swagger avec la sécurité Basic Authentication
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PatientService API",
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


builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

builder.Services
    .AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
        "BasicAuthentication", null);

builder.Services.AddAuthorization();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientAppService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigrationsWithRetry(app);

app.Run();

// Méthode pour appliquer les migrations avec une logique de retry en cas d'échec
static void ApplyMigrationsWithRetry(WebApplication app)
{
    const int retries = 10;
    var delay = TimeSpan.FromSeconds(10);

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PatientDbContext>();

    for (var attempt = 1; attempt <= retries; attempt++)
    {
        try
        {
            dbContext.Database.Migrate();
            return;
        }
        catch
        {
            if (attempt == retries)
            {
                throw;
            }

            Thread.Sleep(delay);
        }
    }
}
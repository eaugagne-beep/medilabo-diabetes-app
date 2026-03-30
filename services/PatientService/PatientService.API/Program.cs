using Microsoft.EntityFrameworkCore;
using PatientService.Infrastructure.Data;
using PatientService.Application.Interfaces;
using PatientService.Infrastructure.Repositories;
using PatientService.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientAppService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();


var retries = 10;
var delay = TimeSpan.FromSeconds(10);

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PatientDbContext>();

    for (int i = 0; i < retries; i++)
    {
        try
        {
            dbContext.Database.Migrate();
            break;
        }
        catch
        {
            if (i == retries - 1)
            {
                throw;
            }

            Thread.Sleep(delay);
        }
    }
}


app.Run();
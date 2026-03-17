using Microsoft.EntityFrameworkCore;
using PatientService.Domain.Entities;

namespace PatientService.Infrastructure.Data;

public class PatientDbContext : DbContext
{
    public PatientDbContext(DbContextOptions<PatientDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
}
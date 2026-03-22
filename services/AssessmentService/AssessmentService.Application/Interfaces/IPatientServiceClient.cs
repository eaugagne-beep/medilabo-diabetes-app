using AssessmentService.Domain.Models;

namespace AssessmentService.Application.Interfaces;

public interface IPatientServiceClient
{
    Task<PatientInfo?> GetPatientByIdAsync(int patientId);
}
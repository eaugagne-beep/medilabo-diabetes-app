using AssessmentService.Domain.Models;

namespace AssessmentService.Application.Interfaces;

public interface INoteServiceClient
{
    Task<IEnumerable<PatientNoteInfo>> GetNotesByPatientIdAsync(int patientId);
}
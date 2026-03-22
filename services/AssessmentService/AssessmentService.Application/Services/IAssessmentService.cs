using AssessmentService.Application.DTOs;

namespace AssessmentService.Application.Services;

public interface IAssessmentService
{
    Task<AssessmentResultDto> AssessRiskAsync(int patientId);
}
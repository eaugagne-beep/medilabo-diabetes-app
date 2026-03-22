namespace AssessmentService.Application.DTOs;

public class AssessmentResultDto
{
    public int PatientId { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
}
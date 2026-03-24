namespace PatientFront.Models;

public class AssessmentResultViewModel
{
	public int PatientId { get; set; }
	public string RiskLevel { get; set; } = string.Empty;
}
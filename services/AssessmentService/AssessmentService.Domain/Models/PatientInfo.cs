namespace AssessmentService.Domain.Models;

public class PatientInfo
{
    public int Id { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
}
namespace PatientFront.Models;

public class PatientDetailsViewModel
{
    public PatientViewModel Patient { get; set; } = new();
    public List<PatientNoteViewModel> Notes { get; set; } = new();
    public string RiskLevel { get; set; } = string.Empty;
}
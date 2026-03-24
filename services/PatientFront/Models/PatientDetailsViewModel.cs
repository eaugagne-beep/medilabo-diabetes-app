namespace PatientFront.Models;

public class PatientDetailsViewModel
{
    public PatientViewModel Patient { get; set; } = new PatientViewModel();
    public List<PatientNoteViewModel> Notes { get; set; } = new List<PatientNoteViewModel>();
    public string RiskLevel { get; set; } = string.Empty;
}
namespace PatientFront.Models;

public class PatientDetailsViewModel
{
    public PatientViewModel Patient { get; set; } = new PatientViewModel();
    public List<PatientNoteViewModel> Notes { get; set; } = new List<PatientNoteViewModel>();
}
namespace PatientFront.Models;

public class PatientNoteViewModel
{
    public string Id { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public string Note { get; set; } = string.Empty;
}
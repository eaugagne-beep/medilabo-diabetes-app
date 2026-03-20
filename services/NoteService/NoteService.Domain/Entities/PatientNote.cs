namespace NoteService.Domain.Entities;

public class PatientNote
{
    public string Id { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public string Note { get; set; } = string.Empty;
}
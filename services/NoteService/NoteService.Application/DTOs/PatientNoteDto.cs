namespace NoteService.Application.DTOs;

public class PatientNoteDto
{
    public string Id { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public string Note { get; set; } = string.Empty;
}
namespace NoteService.Application.DTOs;

public class CreatePatientNoteDto
{
    public int PatientId { get; set; }
    public string Note { get; set; } = string.Empty;
}
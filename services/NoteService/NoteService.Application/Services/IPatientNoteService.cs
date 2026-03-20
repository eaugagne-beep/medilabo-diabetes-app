using NoteService.Application.DTOs;

namespace NoteService.Application.Services;

public interface IPatientNoteService
{
    Task<IEnumerable<PatientNoteDto>> GetByPatientIdAsync(int patientId);
    Task<PatientNoteDto> AddAsync(CreatePatientNoteDto createPatientNoteDto);
}
using NoteService.Domain.Entities;

namespace NoteService.Application.Interfaces;

public interface IPatientNoteRepository
{
    Task<IEnumerable<PatientNote>> GetByPatientIdAsync(int patientId);
    Task AddAsync(PatientNote patientNote);
}
using NoteService.Application.DTOs;
using NoteService.Application.Interfaces;
using NoteService.Domain.Entities;

namespace NoteService.Application.Services;

public class PatientNoteAppService : IPatientNoteService
{
    private readonly IPatientNoteRepository _patientNoteRepository;

    public PatientNoteAppService(IPatientNoteRepository patientNoteRepository)
    {
        _patientNoteRepository = patientNoteRepository;
    }

    public async Task<IEnumerable<PatientNoteDto>> GetByPatientIdAsync(int patientId)
    {
        var notes = await _patientNoteRepository.GetByPatientIdAsync(patientId);

        return notes.Select(note => new PatientNoteDto
        {
            Id = note.Id,
            PatientId = note.PatientId,
            Note = note.Note
        });
    }

    public async Task<PatientNoteDto> AddAsync(CreatePatientNoteDto createPatientNoteDto)
    {
        var patientNote = new PatientNote
        {
            PatientId = createPatientNoteDto.PatientId,
            Note = createPatientNoteDto.Note
        };

        await _patientNoteRepository.AddAsync(patientNote);

        return new PatientNoteDto
        {
            Id = patientNote.Id,
            PatientId = patientNote.PatientId,
            Note = patientNote.Note
        };
    }
}
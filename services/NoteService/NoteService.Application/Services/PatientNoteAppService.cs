using NoteService.Application.DTOs;
using NoteService.Application.Interfaces;
using NoteService.Domain.Entities;

namespace NoteService.Application.Services;

public class PatientNoteAppService : IPatientNoteService
{
    private readonly IPatientNoteRepository _patientNoteRepository;

    // Injection du repository de notes de patients
    public PatientNoteAppService(IPatientNoteRepository patientNoteRepository)
    {
        _patientNoteRepository = patientNoteRepository;
    }

    // Récupération des notes d'un patient par son ID
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

    // Ajout d'une nouvelle note pour un patient
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
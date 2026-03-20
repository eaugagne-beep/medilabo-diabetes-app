using MongoDB.Driver;
using NoteService.Application.Interfaces;
using NoteService.Domain.Entities;
using NoteService.Infrastructure.Services;

namespace NoteService.Infrastructure.Repositories;

public class PatientNoteRepository : IPatientNoteRepository
{
    private readonly IMongoCollection<PatientNote> _notesCollection;

    public PatientNoteRepository(NoteMongoService noteMongoService)
    {
        _notesCollection = noteMongoService.NotesCollection;
    }

    public async Task<IEnumerable<PatientNote>> GetByPatientIdAsync(int patientId)
    {
        return await _notesCollection
            .Find(note => note.PatientId == patientId)
            .ToListAsync();
    }

    public async Task AddAsync(PatientNote patientNote)
    {
        await _notesCollection.InsertOneAsync(patientNote);
    }
}
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoteService.Domain.Entities;
using NoteService.Infrastructure.Settings;

namespace NoteService.Infrastructure.Services;

public class NoteMongoService
{
    private readonly IMongoCollection<PatientNote> _notesCollection;

    public NoteMongoService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);

        _notesCollection = mongoDatabase.GetCollection<PatientNote>(mongoDbSettings.Value.CollectionName);
    }

    public IMongoCollection<PatientNote> NotesCollection => _notesCollection;
}
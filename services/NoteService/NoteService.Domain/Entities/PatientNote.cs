using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace NoteService.Domain.Entities;

public class PatientNote
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } 
    public int PatientId { get; set; }
    public string Note { get; set; } = string.Empty;
}
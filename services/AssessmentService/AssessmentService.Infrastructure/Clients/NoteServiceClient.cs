using AssessmentService.Application.Interfaces;
using AssessmentService.Domain.Models;
using Newtonsoft.Json;

namespace AssessmentService.Infrastructure.Clients;

public class NoteServiceClient : INoteServiceClient
{
    private readonly HttpClient _httpClient;

    public NoteServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<PatientNoteInfo>> GetNotesByPatientIdAsync(int patientId)
    {
        var response = await _httpClient.GetAsync($"/api/Notes/patient/{patientId}");

        if (!response.IsSuccessStatusCode)
        {
            return new List<PatientNoteInfo>();
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<PatientNoteInfo>>(json)
               ?? new List<PatientNoteInfo>();
    }
}
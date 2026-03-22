using AssessmentService.Application.Interfaces;
using AssessmentService.Domain.Models;
using Newtonsoft.Json;

namespace AssessmentService.Infrastructure.Clients;

public class PatientServiceClient : IPatientServiceClient
{
    private readonly HttpClient _httpClient;

    public PatientServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PatientInfo?> GetPatientByIdAsync(int patientId)
    {
        var response = await _httpClient.GetAsync($"/api/Patients/{patientId}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<PatientInfo>(json);
    }
}
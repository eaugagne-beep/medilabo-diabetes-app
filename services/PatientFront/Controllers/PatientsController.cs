using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PatientFront.Models;

namespace PatientFront.Controllers;

public class PatientsController : Controller
{
    private readonly HttpClient _httpClient;

    
    public PatientsController()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://gatewayservice:8080")
        };

        var credentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes("admin:admin123"));

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", credentials);
    }

    // Action pour afficher la liste des patients
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("/patients");

        if (!response.IsSuccessStatusCode)
        {
            return View(new List<PatientViewModel>());
        }

        var json = await response.Content.ReadAsStringAsync();
        var patients = JsonConvert.DeserializeObject<List<PatientViewModel>>(json)
            ?? new List<PatientViewModel>();

        return View(patients);
    }

    // Action pour afficher les détails d'un patient
    public async Task<IActionResult> Details(int id)
    {
        var patientResponse = await _httpClient.GetAsync($"/patients/{id}");

        if (!patientResponse.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var patientJson = await patientResponse.Content.ReadAsStringAsync();
        var patient = JsonConvert.DeserializeObject<PatientViewModel>(patientJson);

        if (patient is null)
        {
            return NotFound();
        }

        var notes = await GetPatientNotesAsync(id);
        var riskLevel = await GetRiskLevelAsync(id);

        var viewModel = new PatientDetailsViewModel
        {
            Patient = patient,
            Notes = notes,
            RiskLevel = riskLevel
        };

        return View(viewModel);
    }

    // Méthodes pour récupérer les notes et le niveau de risque
    private async Task<List<PatientNoteViewModel>> GetPatientNotesAsync(int patientId)
    {
        var response = await _httpClient.GetAsync($"/notes/patient/{patientId}");

        if (!response.IsSuccessStatusCode)
        {
            return new List<PatientNoteViewModel>();
        }

        var json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<List<PatientNoteViewModel>>(json)
            ?? new List<PatientNoteViewModel>();
    }

    // Méthode pour récupérer le niveau de risque
    private async Task<string> GetRiskLevelAsync(int patientId)
    {
        var response = await _httpClient.GetAsync($"/assessment/{patientId}");

        if (!response.IsSuccessStatusCode)
        {
            return "Unavailable";
        }

        var json = await response.Content.ReadAsStringAsync();
        var assessment = JsonConvert.DeserializeObject<AssessmentResultViewModel>(json);

        return assessment?.RiskLevel ?? "Unavailable";
    }
}
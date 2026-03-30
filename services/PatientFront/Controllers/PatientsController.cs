using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PatientFront.Models;

namespace PatientFront.Controllers;

public class PatientsController : Controller
{
    private readonly HttpClient _httpClient;

    public PatientsController()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://gatewayservice:8080");
    }

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

    public async Task<IActionResult> Details(int id)
    {
        // 1. Récupérer le patient via Gateway
        var patientResponse = await _httpClient.GetAsync($"/patients/{id}");

        if (!patientResponse.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var patientJson = await patientResponse.Content.ReadAsStringAsync();
        var patient = JsonConvert.DeserializeObject<PatientViewModel>(patientJson);

        if (patient == null)
        {
            return NotFound();
        }

        // 2. Récupérer les notes via Gateway
        var notesResponse = await _httpClient.GetAsync($"/notes/patient/{id}");

        List<PatientNoteViewModel> notes = new();

        if (notesResponse.IsSuccessStatusCode)
        {
            var notesJson = await notesResponse.Content.ReadAsStringAsync();
            notes = JsonConvert.DeserializeObject<List<PatientNoteViewModel>>(notesJson)
                    ?? new List<PatientNoteViewModel>();
        }

        // 3. Récupérer l'assessment directement
        string riskLevel = "Unavailable";

        var assessmentResponse = await _httpClient.GetAsync($"/assessment/{id}");

        if (assessmentResponse.IsSuccessStatusCode)
        {
            var assessmentJson = await assessmentResponse.Content.ReadAsStringAsync();
            var assessment = JsonConvert.DeserializeObject<AssessmentResultViewModel>(assessmentJson);

            if (assessment != null)
            {
                riskLevel = assessment.RiskLevel;
            }
        }

        // 4. Construire le ViewModel complet
        var viewModel = new PatientDetailsViewModel
        {
            Patient = patient,
            Notes = notes,
            RiskLevel = riskLevel
        };

        return View(viewModel);
    }
}
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PatientFront.Models;


namespace PatientFront.Controllers;

[Authorize]
public class PatientsController : Controller
{
    private HttpClient CreateClientWithAuth()
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri("http://gatewayservice:8080")
        };

        var username = HttpContext.Session.GetString("BasicAuthUsername");
        var password = HttpContext.Session.GetString("BasicAuthPassword");

        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
        {
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{username}:{password}"));

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", credentials);
        }

        return client;
    }


    // Action pour afficher la liste des patients
    public async Task<IActionResult> Index()
    {
        var client = CreateClientWithAuth();
        var response = await client.GetAsync("/patients");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return Content($"Erreur API : {(int)response.StatusCode} - {response.StatusCode}\n{errorContent}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var patients = JsonConvert.DeserializeObject<List<PatientViewModel>>(json)
            ?? new List<PatientViewModel>();

        return View(patients);
    }

    // Action pour afficher les détails d'un patient
    public async Task<IActionResult> Details(int id)
    {
        var client = CreateClientWithAuth();
        var patientResponse = await client.GetAsync($"/patients/{id}");

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
        var client = CreateClientWithAuth();
        var response = await client.GetAsync($"/notes/patient/{patientId}");

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
        var client = CreateClientWithAuth();
        var response = await client.GetAsync($"/assessment/{patientId}");

        if (!response.IsSuccessStatusCode)
        {
            return "Unavailable";
        }

        var json = await response.Content.ReadAsStringAsync();
        var assessment = JsonConvert.DeserializeObject<AssessmentResultViewModel>(json);

        return assessment?.RiskLevel ?? "Unavailable";
    }
}
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
        _httpClient.BaseAddress = new Uri("http://localhost:5110");
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
        var response = await _httpClient.GetAsync($"/patients/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        var patient = JsonConvert.DeserializeObject<PatientViewModel>(json);

        if (patient == null)
        {
            return NotFound();
        }

        return View(patient);
    }
}
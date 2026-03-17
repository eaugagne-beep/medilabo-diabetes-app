using Microsoft.AspNetCore.Mvc;
using PatientService.Application.DTOs;
using PatientService.Application.Services;

namespace PatientService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetById(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);

        if (patient == null)
            return NotFound();

        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create(CreatePatientDto createPatientDto)
    {
        var createdPatient = await _patientService.AddAsync(createPatientDto);

        return CreatedAtAction(nameof(GetById), new { id = createdPatient.Id }, createdPatient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePatientDto updatePatientDto)
    {
        var updated = await _patientService.UpdateAsync(id, updatePatientDto);

        if (!updated)
            return NotFound();

        return NoContent();
    }
}
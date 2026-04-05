using Microsoft.AspNetCore.Mvc;
using NoteService.Application.DTOs;
using NoteService.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace NoteService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly IPatientNoteService _patientNoteService;

    public NotesController(IPatientNoteService patientNoteService)
    {
        _patientNoteService = patientNoteService;
    }

    [HttpGet("patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<PatientNoteDto>>> GetByPatientId(int patientId)
    {
        var notes = await _patientNoteService.GetByPatientIdAsync(patientId);
        return Ok(notes);
    }

    [HttpPost]
    public async Task<ActionResult<PatientNoteDto>> Create(CreatePatientNoteDto createPatientNoteDto)
    {
        var createdNote = await _patientNoteService.AddAsync(createPatientNoteDto);

        return CreatedAtAction(nameof(GetByPatientId), new { patientId = createdNote.PatientId }, createdNote);
    }
}
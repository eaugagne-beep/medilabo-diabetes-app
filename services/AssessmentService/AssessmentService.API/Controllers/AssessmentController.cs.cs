using AssessmentService.Application.DTOs;
using AssessmentService.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AssessmentService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AssessmentController : ControllerBase
{
    private readonly IAssessmentService _assessmentService;

    public AssessmentController(IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }

    [HttpGet("{patientId}")]
    public async Task<ActionResult<AssessmentResultDto>> GetAssessment(int patientId)
    {
        var result = await _assessmentService.AssessRiskAsync(patientId);
        return Ok(result);
    }
}
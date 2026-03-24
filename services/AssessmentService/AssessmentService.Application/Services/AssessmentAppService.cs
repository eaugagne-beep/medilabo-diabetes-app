using AssessmentService.Application.DTOs;
using AssessmentService.Application.Interfaces;
using AssessmentService.Domain.Models;

namespace AssessmentService.Application.Services;

public class AssessmentAppService : IAssessmentService
{
    private readonly IPatientServiceClient _patientServiceClient;
    private readonly INoteServiceClient _noteServiceClient;

    private static readonly string[] TriggerTerms =
    {
        "hémoglobine a1c",
        "microalbumine",
        "taille",
        "poids",
        "fumeur",
        "fumeuse",
        "anormal",
        "cholestérol",
        "vertiges",
        "rechute",
        "réaction",
        "anticorps"
    };

    public AssessmentAppService(
        IPatientServiceClient patientServiceClient,
        INoteServiceClient noteServiceClient)
    {
        _patientServiceClient = patientServiceClient;
        _noteServiceClient = noteServiceClient;
    }

    public async Task<AssessmentResultDto> AssessRiskAsync(int patientId)
    {
        var patient = await _patientServiceClient.GetPatientByIdAsync(patientId);

        if (patient == null)
        {
            return new AssessmentResultDto
            {
                PatientId = patientId,
                RiskLevel = "Patient not found"
            };
        }

        var notes = await _noteServiceClient.GetNotesByPatientIdAsync(patientId);

        var age = CalculateAge(patient.DateOfBirth);
        var triggerCount = CountTriggerTerms(notes);

        var riskLevel = DetermineRiskLevel(age, patient.Gender, triggerCount);

        return new AssessmentResultDto
        {
            PatientId = patientId,
            RiskLevel = riskLevel
        };
    }

    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    private static int CountTriggerTerms(IEnumerable<PatientNoteInfo> notes)
    {
        var count = 0;

        foreach (var note in notes)
        {
            var noteText = note.Note.ToLowerInvariant();

            foreach (var trigger in TriggerTerms)
            {
                if (noteText.Contains(trigger))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private static string DetermineRiskLevel(int age, string gender, int triggerCount)
    {
        var normalizedGender = gender.Trim().ToUpperInvariant();

        if (triggerCount == 0)
        {
            return "None";
        }

        if (age > 30)
        {
            if (triggerCount >= 8)
            {
                return "EarlyOnset"; 
            }

            if (triggerCount >= 6)
            {
                return "InDanger";
            }

            if (triggerCount >= 2)
            {
                return "Borderline";
            }

            return "None";
        }

        if (normalizedGender == "M")
        {
            if (triggerCount >= 5)
            {
                return "EarlyOnset";
            }

            if (triggerCount >= 3)
            {
                return "InDanger";
            }

            return "None";
        }

        if (normalizedGender == "F")
        {
            if (triggerCount >= 7)
            {
                return "EarlyOnset";
            }

            if (triggerCount >= 4)
            {
                return "InDanger";
            }

            return "None";
        }

        return "None";
    }
}
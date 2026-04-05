using AssessmentService.Application.DTOs;
using AssessmentService.Application.Interfaces;
using AssessmentService.Domain.Models;

namespace AssessmentService.Application.Services;

public class AssessmentAppService : IAssessmentService
{
    // Liste des termes déclencheurs à rechercher dans les notes du patient
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

    private readonly IPatientServiceClient _patientServiceClient;
    private readonly INoteServiceClient _noteServiceClient;

    
    public AssessmentAppService(
        IPatientServiceClient patientServiceClient,
        INoteServiceClient noteServiceClient)
    {
        _patientServiceClient = patientServiceClient;
        _noteServiceClient = noteServiceClient;
    }

    // Méthode principale pour évaluer le risque du patient
    public async Task<AssessmentResultDto> AssessRiskAsync(int patientId)
    {
        var patient = await _patientServiceClient.GetPatientByIdAsync(patientId);

        if (patient is null)
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
   
    // Calcul de l'âge du patient à partir de sa date de naissance
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

    // Comptage des termes déclencheurs dans les notes du patient
    private static int CountTriggerTerms(IEnumerable<PatientNoteInfo> notes)
    {
        var count = 0;

        foreach (var note in notes)
        {
            var noteText = (note.Note ?? string.Empty).ToLowerInvariant();

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

    // Détermination du niveau de risque
    private static string DetermineRiskLevel(int age, string? gender, int triggerCount)
    {
        var normalizedGender = (gender ?? string.Empty).Trim().ToUpperInvariant();

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

            return triggerCount >= 2 ? "Borderline" : "None";
        }

        if (normalizedGender == "M")
        {
            if (triggerCount >= 5)
            {
                return "EarlyOnset";
            }

            return triggerCount >= 3 ? "InDanger" : "None";
        }

        if (normalizedGender == "F")
        {
            if (triggerCount >= 7)
            {
                return "EarlyOnset";
            }

            return triggerCount >= 4 ? "InDanger" : "None";
        }

        return "None";
    }
}
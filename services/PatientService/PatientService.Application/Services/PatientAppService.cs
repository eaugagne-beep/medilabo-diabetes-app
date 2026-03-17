using PatientService.Application.DTOs;
using PatientService.Application.Interfaces;
using PatientService.Domain.Entities;

namespace PatientService.Application.Services;

public class PatientAppService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientAppService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync()
    {
        var patients = await _patientRepository.GetAllAsync();

        return patients.Select(p => new PatientDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Gender = p.Gender,
            Address = p.Address,
            Phone = p.Phone
        });
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
            return null;

        return new PatientDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            Address = patient.Address,
            Phone = patient.Phone
        };
    }

    public async Task<PatientDto> AddAsync(CreatePatientDto createPatientDto)
    {
        var patient = new Patient
        {
            FirstName = createPatientDto.FirstName,
            LastName = createPatientDto.LastName,
            DateOfBirth = createPatientDto.DateOfBirth,
            Gender = createPatientDto.Gender,
            Address = createPatientDto.Address,
            Phone = createPatientDto.Phone
        };

        await _patientRepository.AddAsync(patient);

        return new PatientDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            Address = patient.Address,
            Phone = patient.Phone
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdatePatientDto updatePatientDto)
    {
        var existingPatient = await _patientRepository.GetByIdAsync(id);

        if (existingPatient == null)
            return false;

        existingPatient.FirstName = updatePatientDto.FirstName;
        existingPatient.LastName = updatePatientDto.LastName;
        existingPatient.DateOfBirth = updatePatientDto.DateOfBirth;
        existingPatient.Gender = updatePatientDto.Gender;
        existingPatient.Address = updatePatientDto.Address;
        existingPatient.Phone = updatePatientDto.Phone;

        await _patientRepository.UpdateAsync(existingPatient);

        return true;
    }
}
using PatientService.Application.DTOs;

namespace PatientService.Application.Services;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllAsync();
    Task<PatientDto?> GetByIdAsync(int id);
    Task<PatientDto> AddAsync(CreatePatientDto createPatientDto);
    Task<bool> UpdateAsync(int id, UpdatePatientDto updatePatientDto);
}
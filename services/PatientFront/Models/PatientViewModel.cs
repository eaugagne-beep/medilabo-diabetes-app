namespace PatientFront.Models;

public class PatientViewModel
{
	public int Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public DateTime DateOfBirth { get; set; }
	public string Gender { get; set; } = string.Empty;
	public string? Address { get; set; }
	public string? Phone { get; set; }
}
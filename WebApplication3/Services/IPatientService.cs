using WebApplication3.Models;

namespace WebApplication3.Services;

public interface IPatientService
{
   public Task<IEnumerable<Patient>> GetPatients(string? query);
   public Task AssignBedToPatient(string pesel, Bed bed);
}
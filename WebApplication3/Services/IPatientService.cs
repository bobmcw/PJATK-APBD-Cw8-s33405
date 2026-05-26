using WebApplication3.DTOs;
using WebApplication3.Models;

namespace WebApplication3.Services;

public interface IPatientService
{
   public Task<IEnumerable<Patient>> GetPatients(CancellationToken token, string? query);
   public Task AssignBedToPatient(CancellationToken token, string pesel, BedAssignmentRequest req);
}
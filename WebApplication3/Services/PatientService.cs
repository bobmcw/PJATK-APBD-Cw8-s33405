using Microsoft.EntityFrameworkCore;
using WebApplication3.Infrastructure;
using WebApplication3.Models;

namespace WebApplication3.Services;

public class PatientService(MasterContext context) : IPatientService
{
    public async Task<IEnumerable<Patient>> GetPatients(string? query)
    {
        query ??= "";
        query = query.ToLower();
        return await context.Patients
            .Where(p => p.FirstName.Contains(query) || p.LastName.Contains(query))
            .Select(p => p).ToListAsync();

    }

    public Task AssignBedToPatient(string pesel, Bed bed)
    {
        throw new NotImplementedException();
    }
}
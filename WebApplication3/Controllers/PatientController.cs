using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(IPatientService service) : ControllerBase
{ 
   [HttpGet]
   [HttpGet("{query}")]
   public async Task<IEnumerable<Patient>> GetPatients(CancellationToken token, string? query = null)
   {
       return await service.GetPatients(query);
   }
}
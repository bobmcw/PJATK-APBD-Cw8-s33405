using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs;
using WebApplication3.Exceptions;
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
       return await service.GetPatients(token,query);
   }

   [HttpPost("{pesel}/bedAssignments")]
   public async Task<IActionResult> AssignBedToPatient(CancellationToken token, [FromBody] BedAssignmentRequest req, [FromRoute] string pesel)
   {
       try
       {
           await service.AssignBedToPatient(token, pesel, req);
           return NoContent();
       }
       catch (NotFoundException e)
       {
           return NotFound(e.Message);
       }
   }
}
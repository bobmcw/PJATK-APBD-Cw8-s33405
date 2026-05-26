using Microsoft.EntityFrameworkCore;
using WebApplication3.DTOs;
using WebApplication3.Exceptions;
using WebApplication3.Infrastructure;
using WebApplication3.Models;

namespace WebApplication3.Services;

public class PatientService(MasterContext context) : IPatientService
{
    public async Task<IEnumerable<Patient>> GetPatients(CancellationToken token, string? query)
    {
        query ??= "";
        query = query.ToLower();
        var list = await context.Patients
            .Where(p => p.FirstName.Contains(query) || p.LastName.Contains(query))
            .Select(p => p).ToListAsync(token);
        if (list.Count == 0)
        {
            throw new NotFoundException("no patient matches the query");
        }

        return list;
    }

    public async Task AssignBedToPatient(CancellationToken token, string pesel, BedAssignmentRequest req)
    {
        if (!context.Patients.Any(e => e.Pesel == pesel))
        {
            throw new NotFoundException("no patient with provided PESEL");
        }
        var reqDays = (req.To - req.From);
        var list = await context.BedAssignments.Where(e => e.From < (req.From - reqDays) || e.To > req.To)
            .Join(context.Beds, ba => ba.BedId, bed => bed.Id, (ba, bed) => new { BedAssignment = ba, Bed = bed })
            .Join(context.BedTypes, e => e.Bed.BedTypeId, type => type.Id,
                (e, type) => new { BedAssignment = e.BedAssignment, Bed = e.Bed, BedType = type })
            .Join(context.Rooms, e => e.Bed.RoomId, room => room.Id, (e, room) =>
                new { Bed = e.Bed, BedAssignment = e.BedAssignment, BedType = e.BedType, Room = room })
            .Join(context.Wards, e => e.Room.WardId, ward => ward.Id, (e, ward) =>
                new { Room = e.Room, BedAssignment = e.BedAssignment, Bed = e.Bed, BedType = e.BedType, Ward = ward })
            .Where(e => e.Ward.Name == req.Ward)
            .Where(e => e.BedType.Name == req.BedType)
            .ToListAsync(token);
        if (list.Count == 0)
        {
            throw new NotFoundException("No bed matching the query");
        }

        var id = context.BedAssignments.Count() + 1;
        var first = list.First();
        context.BedAssignments.Add(new BedAssignment
        {
            BedId = first.Bed.Id,
            From = req.From,
            To = req.To,
            Id = id,
            PatientPesel = pesel
        });
    }
}
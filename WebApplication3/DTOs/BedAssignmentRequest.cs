using WebApplication3.Models;

namespace WebApplication3.DTOs;

public record BedAssignmentRequest(
    DateTime From,
    DateTime To,
    string BedType,
    string Ward
);
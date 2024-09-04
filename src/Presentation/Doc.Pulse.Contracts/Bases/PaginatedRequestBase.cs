using Doc.Pulse.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Doc.Pulse.Contracts.Bases;

public class PaginatedRequestBase : IPaginatedRequest
{
    [FromQuery][Range(0, int.MaxValue)] public int? SkipAmount { get; set; }
    [FromQuery][Range(0, 9999999)] public int? TakeAmount { get; set; } = 999999;
    [FromQuery][StringLength(200)] public string? SortBy { get; set; }
    [FromQuery][StringLength(1024)] public string? Filter { get; set; }
    [FromQuery] public bool? SortDesc { get; set; }
}

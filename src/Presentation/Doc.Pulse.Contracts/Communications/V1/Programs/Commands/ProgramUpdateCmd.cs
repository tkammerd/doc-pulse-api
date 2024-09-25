using Doc.Pulse.Contracts.Communications.V1.Programs.Queries;

namespace Doc.Pulse.Contracts.Communications.V1.Programs.Commands;

public class ProgramUpdateCmd
{
    public int Id { get; set; }
    public string ProgramCode { get; set; } = null!;
    public string? ProgramName { get; set; }
    public string? ProgramDescription { get; set; }
    public bool Inactive { get; set; } = false;
}
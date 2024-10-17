namespace Doc.Pulse.Contracts.Communications.V1.Programs.Queries;

public class ProgramGetByIdResponse
{
    public int? Id { get; set; } = default!;

    public string ProgramCode { get; set; } = null!;
    public string? ProgramName { get; set; }
    public string? ProgramDescription { get; set; }
    public bool Inactive { get; set; } = false;
    public byte[]? RowVersion { get; set; } = null;
}

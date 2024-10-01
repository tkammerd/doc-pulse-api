namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class ProgramDto
{
    public int ProgramId { get; set; }
    public string ProgramCode { get; set; } = null!;
    public string? ProgramName { get; set; }
    public string? ProgramDescription { get; set; }
    public bool Inactive { get; set; } = false;

    public T ToEntity<T>() where T : Core.Entities.Program, new()
    {
        return new T()
        {
            Id = ProgramId,
            ProgramCode = ParsingHelpers.TrimPreventNull(ProgramCode,"ProgramCode"),
            ProgramName = ParsingHelpers.TrimAllowNull(ProgramName),
            ProgramDescription = ParsingHelpers.TrimAllowNull(ProgramDescription),
            Inactive = Inactive
        };
    }
}
namespace Doc.Pulse.Contracts.Communications.V1.Agencies.Commands;

public class AgencyCreateCmd
{
    public string AgencyName { get; set; } = null!;
    public bool Inactive { get; set; } = false;
}

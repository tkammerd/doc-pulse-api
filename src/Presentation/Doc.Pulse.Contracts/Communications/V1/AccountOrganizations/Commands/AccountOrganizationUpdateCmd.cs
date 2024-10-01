namespace Doc.Pulse.Contracts.Communications.V1.AccountOrganizations.Commands;

public class AccountOrganizationUpdateCmd
{
    public int Id { get; set; }
    public string AccountOrganizationNumber { get; set; } = null!;
    public string? CostCenterDescription { get; set; }
    public bool Inactive { get; set; } = false;
}
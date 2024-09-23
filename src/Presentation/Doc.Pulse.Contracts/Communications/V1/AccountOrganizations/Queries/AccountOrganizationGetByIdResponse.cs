namespace Doc.Pulse.Contracts.Communications.V1.AccountOrganizations.Queries;

public class AccountOrganizationGetByIdResponse
{
    public int? Id { get; set; } = default!;

    public string AccountOrganizationNumber { get; set; } = null!;
    public string? CostCenterDescription { get; set; }
    public bool Inactive { get; set; } = false;
}

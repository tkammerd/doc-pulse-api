using Doc.Pulse.Core.Entities;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class AccountOrganizationDto
{
    public int AccountOrganizationId { get; set; }
    public string AccountOrganizationNumber { get; set; } = null!;
    public string? CostCenterDescription { get; set; }
    public bool Inactive { get; set; } = false;

    public T ToEntity<T>() where T : AccountOrganization, new()
    {
        return new T()
        {
            Id = AccountOrganizationId,
            AccountOrganizationNumber = ParsingHelpers.TrimPreventNull(AccountOrganizationNumber, "AccountOrganizationNumber"),
            CostCenterDescription = ParsingHelpers.TrimAllowNull(CostCenterDescription),
            Inactive = Inactive
        };
    }
}
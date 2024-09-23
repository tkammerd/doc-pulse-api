using Doc.Pulse.Contracts.Communications.V1.AccountOrganizations.Dtos;
using Doc.Pulse.Contracts.Interfaces;

namespace Doc.Pulse.Contracts.Communications.V1.AccountOrganizations.Queries;

public class AccountOrganizationListResponse : PaginatedResponseBase
{
    public List<AccountOrganizationListDto> Items { get; set; } = new();
}

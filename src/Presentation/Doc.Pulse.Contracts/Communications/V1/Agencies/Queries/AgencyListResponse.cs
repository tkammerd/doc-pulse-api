using Doc.Pulse.Contracts.Communications.V1.Agencies.Dtos;
using Doc.Pulse.Contracts.Interfaces;

namespace Doc.Pulse.Contracts.Communications.V1.Agencies.Queries;

public class AgencyListResponse : PaginatedResponseBase
{
    public List<AgencyListDto> Items { get; set; } = new();
}

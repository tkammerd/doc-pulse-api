using Doc.Pulse.Contracts.Communications.V1.Rfps.Dtos;
using Doc.Pulse.Contracts.Interfaces;

namespace Doc.Pulse.Contracts.Communications.V1.Rfps.Queries;

public class RfpListResponse : PaginatedResponseBase
{
    public List<RfpListDto> Items { get; set; } = new();
}

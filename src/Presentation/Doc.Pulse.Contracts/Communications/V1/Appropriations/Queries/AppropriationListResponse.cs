using Doc.Pulse.Contracts.Communications.V1.Appropriations.Dtos;
using Doc.Pulse.Contracts.Interfaces;

namespace Doc.Pulse.Contracts.Communications.V1.Appropriations.Queries;

public class AppropriationListResponse : PaginatedResponseBase
{
    public List<AppropriationListDto> Items { get; set; } = new();
}

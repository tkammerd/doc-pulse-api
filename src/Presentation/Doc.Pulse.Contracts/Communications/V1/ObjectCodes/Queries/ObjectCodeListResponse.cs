using Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Dtos;
using Doc.Pulse.Contracts.Interfaces;

namespace Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Queries;

public class ObjectCodeListResponse : PaginatedResponseBase
{
    public List<ObjectCodeListDto> Items { get; set; } = new();
}

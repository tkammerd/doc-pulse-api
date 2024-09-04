using Doc.Pulse.Contracts.Communications.V1.ApiInformation.Dtos;
using Doc.Pulse.Contracts.Interfaces;

namespace Doc.Pulse.Contracts.Communications.V1.ApiInformation.Queries;

public class ApiInformationListResponse : PaginatedResponseBase
{
    public List<ApiInformationDto> Items { get; set; } = new();
}

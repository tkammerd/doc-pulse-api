using Doc.Pulse.Contracts.Communications.V1.Programs.Dtos;
using Doc.Pulse.Contracts.Interfaces;

namespace Doc.Pulse.Contracts.Communications.V1.Programs.Queries;

public class ProgramListResponse : PaginatedResponseBase
{
    public List<ProgramListDto> Items { get; set; } = new();
}

using Doc.Pulse.Contracts.Communications.V1.CodeCategories.Dtos;
using Doc.Pulse.Contracts.Interfaces;

namespace Doc.Pulse.Contracts.Communications.V1.CodeCategories.Queries;

public class CodeCategoryListResponse : PaginatedResponseBase
{
    public List<CodeCategoryListDto> Items { get; set; } = new();
}

namespace Doc.Pulse.Contracts.Communications.V1.CodeCategories.Queries;

public class CodeCategoryGetByIdResponse
{
    public int? Id { get; set; } = default!;

    public int CategoryNumber { get; set; }
    public string CategoryShortName { get; set; } = null!;
    public string? CategoryName { get; set; }
    public bool Inactive { get; set; } = false;
}

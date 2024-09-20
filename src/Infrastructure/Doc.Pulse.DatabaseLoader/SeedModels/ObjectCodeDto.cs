using Doc.Pulse.Core.Entities;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class ObjectCodeDto
{
    public int CodeNumber { get; set; }
    public string CodeName { get; set; } = null!;
    public int? CodeCategoryId { get; set; }
    public bool Inactive { get; set; } = false;

    public T ToEntity<T>() where T : ObjectCode, new()
    {
        return new T()
        {
            CodeNumber = CodeNumber,
            CodeName = ParsingHelpers.TrimPreventNull(CodeName, "CodeName"),
            CodeCategoryId = CodeCategoryId,
            Inactive = Inactive
        };
    }
}
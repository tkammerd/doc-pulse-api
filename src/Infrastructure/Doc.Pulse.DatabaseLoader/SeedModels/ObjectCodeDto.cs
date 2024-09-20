using Doc.Pulse.Core.Entities;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class ObjectCodeDto
{
    public int ObjectCodeId { get; set; }
    public int CodeNumber { get; set; }
    public string CodeName { get; set; } = null!;
    public int? CodeCategoryId { get; set; }
    public bool Inactive { get; set; } = false;

    public T ToEntity<T>() where T : ObjectCode, new()
    {
        return new T()
        {
            Id = ObjectCodeId,
            CodeNumber = CodeNumber,
            CodeName = ParsingHelpers.TrimPreventNull(CodeName, "CodeName"),
            CodeCategoryId = CodeCategoryId,
            Inactive = Inactive
        };
    }
}
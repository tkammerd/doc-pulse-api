using Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Queries;

namespace Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Commands;

public class ObjectCodeUpdateCmd
{
    public int Id { get; set; }
    public int CodeNumber { get; set; } = default!;
    public string CodeName { get; set; } = default!;
    public int? CodeCategoryId { get; set; } = default!;
    public bool Inactive { get; set; } = default!;
}
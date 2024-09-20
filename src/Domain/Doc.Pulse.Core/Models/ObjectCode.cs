using Doc.Pulse.Core.Attributes;

namespace Doc.Pulse.Core.Entities;

public partial class ObjectCode
{

    public int CodeNumber { get; set; }

    [PaginateFilterAttribute]
    public string CodeName { get; set; } = null!;

    public int? CodeCategoryId { get; set; }

    public bool Inactive { get; set; } = false;

    public virtual CodeCategory? CodeCategory { get; set; }
}
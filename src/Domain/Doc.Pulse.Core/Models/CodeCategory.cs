using Doc.Pulse.Core.Attributes;

namespace Doc.Pulse.Core.Entities;

public partial class CodeCategory
{
    public int CategoryNumber { get; set; }

    [PaginateFilterAttribute]
    public string CategoryShortName { get; set; } = null!;

    [PaginateFilterAttribute]
    public string? CategoryName { get; set; }

    public bool Inactive { get; set; } = false;

    public virtual ICollection<ObjectCode>? ObjectCodes { get; set; } = [];
}
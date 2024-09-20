using Doc.Pulse.Core.Attributes;

namespace Doc.Pulse.Core.Entities;

public partial class Vendor
{
    [PaginateFilterAttribute]
    public string VendorName { get; set; } = null!;

    public bool Inactive { get; set; } = false;

    public virtual ICollection<RFP> Rfps { get; set; } = [];
}
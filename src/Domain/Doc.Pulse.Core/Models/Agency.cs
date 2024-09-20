using Doc.Pulse.Core.Attributes;

namespace Doc.Pulse.Core.Entities;

public partial class Agency
{
    [PaginateFilterAttribute]
    public string AgencyName { get; set; } = null!;

    public bool Inactive { get; set; } = false;

    public virtual ICollection<RFP> Rfps { get; set; } = [];
}
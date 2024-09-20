using Doc.Pulse.Core.Attributes;

namespace Doc.Pulse.Core.Entities;

public partial class AccountOrganization
{
    public string AccountOrganizationNumber { get; set; } = null!;

    [PaginateFilterAttribute]
    public string? CostCenterDescription { get; set; }
    
    public bool Inactive { get; set; } = false;
    
    public virtual ICollection<RFP> Rfps { get; set; } = [];
}
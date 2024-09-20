using Doc.Pulse.Core.Attributes;

namespace Doc.Pulse.Core.Entities;

public partial class Program
{
    [PaginateFilterAttribute]
    public string ProgramCode { get; set; } = null!;

    [PaginateFilterAttribute]
    public string? ProgramName { get; set; }

    [PaginateFilterAttribute]
    public string? ProgramDescription { get; set; }

    public bool Inactive { get; set; } = false;

    public virtual ICollection<RFP> Rfps { get; set; } = [];

    public virtual ICollection<Appropriation> Appropriations { get; set; } = [];
}
using Doc.Pulse.Core.Attributes;

namespace Doc.Pulse.Core.Entities;

public partial class Appropriation
{
    [PaginateFilterAttribute]
    public string Facility { get; set; } = null!;

    [PaginateFilterAttribute]
    public int FiscalYear { get; set; }

    public int ProgramId { get; set; }

    public int ObjectCodeId { get; set; }

    public decimal? CurrentModifiedAmount { get; set; }

    public decimal? PreEncumberedAmount { get; set; }

    public decimal? EncumberedAmount { get; set; }

    public decimal? ExpendedAmount { get; set; }

    public decimal? ProjectedAmount { get; set; }

    public decimal? PriorYearActualAmount { get; set; }

    public decimal? TotalObligated { get; set; }

    public virtual Program? Program { get; set; }

    public virtual ObjectCode? ObjectCode { get; set; }
}
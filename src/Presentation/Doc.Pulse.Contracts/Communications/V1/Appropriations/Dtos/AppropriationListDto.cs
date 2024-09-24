namespace Doc.Pulse.Contracts.Communications.V1.Appropriations.Dtos;

public class AppropriationListDto
{
    public int? Id { get; set; } = default!;

    public string Facility { get; set; } = null!;
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
}
namespace Doc.Pulse.Contracts.Communications.V1.Appropriations.Commands;

public class AppropriationUpdateCmd
{
    public int Id { get; set; }
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
    public byte[]? RowVersion { get; set; }
}
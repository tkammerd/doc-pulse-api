using Doc.Pulse.Core.Entities;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class AppropriationDto
{
    public int AppropriationId { get; set; }
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

    public T ToEntity<T>() where T : Appropriation, new()
    {
        return new T()
        {
            Id = AppropriationId,
            Facility = ParsingHelpers.TrimPreventNull(Facility,"Facility"),
            FiscalYear = FiscalYear,
            ProgramId = ProgramId,
            ObjectCodeId = ObjectCodeId,
            CurrentModifiedAmount = CurrentModifiedAmount,
            PreEncumberedAmount = PreEncumberedAmount,
            EncumberedAmount = EncumberedAmount,
            ExpendedAmount = ExpendedAmount,
            ProjectedAmount = ProjectedAmount,
            PriorYearActualAmount = PriorYearActualAmount,
            TotalObligated = TotalObligated
        };
    }
}
using Doc.Pulse.Core.Entities;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class RfpDto
{
    public int RfpId { get; set; }
    public int FiscalYear { get; set; }
    public string RfpNumber { get; set; } = null!;
    public string Facility { get; set; } = null!;
    public DateTimeOffset? RfpDate { get; set; }
    public string? Description { get; set; }
    public int ObjectCodeId { get; set; }
    public int VendorId { get; set; }
    public int AgencyId { get; set; }
    public int AccountOrganizationId { get; set; }
    public int ProgramId { get; set; }
    public string? PurchaseOrderNumber { get; set; }
    public decimal? AmountObligated { get; set; }
    public string? Completed { get; set; }
    public string? CheckOrDocumentNumber { get; set; }
    public string? Comments { get; set; }
    public string? ReportingCategory { get; set; }
    public string? VerifiedOnIsis { get; set; }
    public string? RequestedBy { get; set; }

    public T ToEntity<T>() where T : Rfp, new()
    {
        return new T()
        {
            Id = RfpId,
            FiscalYear = FiscalYear,
            RfpNumber = ParsingHelpers.TrimPreventNull(RfpNumber, "RfpNumber"),
            Facility = ParsingHelpers.TrimPreventNull(Facility, "Facility"),
            RfpDate = RfpDate,
            Description = ParsingHelpers.TrimAllowNull(Description),
            ObjectCodeId = ObjectCodeId,
            VendorId = VendorId,
            AgencyId = AgencyId,
            AccountOrganizationId = AccountOrganizationId,
            ProgramId = ProgramId,
            PurchaseOrderNumber = ParsingHelpers.TrimAllowNull(PurchaseOrderNumber),
            AmountObligated = AmountObligated,
            Completed = ParsingHelpers.TrimAllowNull(Completed),
            CheckOrDocumentNumber = ParsingHelpers.TrimAllowNull(CheckOrDocumentNumber),
            Comments = ParsingHelpers.TrimAllowNull(Comments),
            ReportingCategory = ParsingHelpers.TrimAllowNull(ReportingCategory),
            VerifiedOnIsis = ParsingHelpers.TrimAllowNull(VerifiedOnIsis),
            RequestedBy = ParsingHelpers.TrimAllowNull(RequestedBy)
        };
    }
}
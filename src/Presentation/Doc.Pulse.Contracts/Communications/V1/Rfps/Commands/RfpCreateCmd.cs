namespace Doc.Pulse.Contracts.Communications.V1.Rfps.Commands;

public class RfpCreateCmd
{
    public string Facility { get; set; } = null!;
    public int FiscalYear { get; set; }
    public string RfpNumber { get; set; } = null!;
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
}

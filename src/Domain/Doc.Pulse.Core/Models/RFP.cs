using Doc.Pulse.Core.Attributes;

namespace Doc.Pulse.Core.Entities;

public partial class Rfp
{
    [PaginateFilterAttribute]
    public int FiscalYear { get; set; }

    [PaginateFilterAttribute]
    public string RfpNumber { get; set; } = null!;

    [PaginateFilterAttribute]
    public string Facility { get; set; } = null!;

    [PaginateFilterAttribute]
    public DateTimeOffset? RfpDate { get; set; }

    [PaginateFilterAttribute]
    public string? Description { get; set; }

    public int ObjectCodeId { get; set; }

    public int VendorId { get; set; }

    public int AgencyId { get; set; }

    public int AccountOrganizationId { get; set; }

    public int ProgramId { get; set; }

    [PaginateFilterAttribute]
    public string? PurchaseOrderNumber { get; set; }

    public decimal? AmountObligated { get; set; }

    public string? Completed { get; set; }

    [PaginateFilterAttribute]
    public string? CheckOrDocumentNumber { get; set; }

    [PaginateFilterAttribute]
    public string? Comments { get; set; }

    [PaginateFilterAttribute]
    public string? ReportingCategory { get; set; }

    public string? VerifiedOnIsis { get; set; }

    [PaginateFilterAttribute]
    public string? RequestedBy { get; set; }

    public virtual ObjectCode? ObjectCode { get; set; }

    public virtual Vendor? Vendor { get; set; }

    public virtual Agency? Agency { get; set; }

    public virtual AccountOrganization? AccountOrganization { get; set; }

    public virtual Program? Program { get; set; }

    public virtual ICollection<Receipt> Receipts { get; set; } = new HashSet<Receipt>();

}
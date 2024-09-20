using Doc.Pulse.Core.Attributes;

namespace Doc.Pulse.Core.Entities;

public partial class Receipt
{
    [PaginateFilterAttribute]
    public int FiscalYear { get; set; }

    public int ReceiptNumber { get; set; }

    public int? RfpId { get; set; }

    [PaginateFilterAttribute]
    public DateTimeOffset? ReceiptDate { get; set; }

    public decimal? ReceivingReportAmount { get; set; }

    public decimal? AmountInIsis {  get; set; }

    public string? ReceiverNumber { get; set; } = null!;

    [PaginateFilterAttribute]
    public string? CheckNumber { get; set; } = null!;

    [PaginateFilterAttribute]
    public DateTimeOffset? CheckDate { get; set; }

    public virtual RFP? Rfp { get; set; }
}
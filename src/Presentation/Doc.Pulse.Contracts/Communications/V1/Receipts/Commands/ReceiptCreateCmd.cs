namespace Doc.Pulse.Contracts.Communications.V1.Receipts.Commands;

public class ReceiptCreateCmd
{
    public string Facility { get; set; } = null!;
    public int FiscalYear { get; set; }
    public int ReceiptNumber { get; set; }
    public int? RfpId { get; set; }
    public DateTimeOffset? ReceiptDate { get; set; }
    public decimal? ReceivingReportAmount { get; set; }
    public decimal? AmountInIsis { get; set; }
    public string? ReceiverNumber { get; set; } = null!;
    public string? CheckNumber { get; set; } = null!;
    public DateTimeOffset? CheckDate { get; set; }
}

using Doc.Pulse.Core.Attributes;
using Doc.Pulse.Core.Entities;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class ReceiptDto
{
    public int ReceiptId { get; set; }
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

    public T ToEntity<T>() where T : Receipt, new()
    {
        return new T()
        {
            Id = ReceiptId,
            Facility = Facility,
            FiscalYear = FiscalYear,
            ReceiptNumber = ReceiptNumber,
            RfpId = RfpId,
            ReceiptDate = ReceiptDate,
            ReceivingReportAmount = ReceivingReportAmount,
            AmountInIsis = AmountInIsis,
            ReceiverNumber = ParsingHelpers.TrimAllowNull(ReceiverNumber),
            CheckNumber = ParsingHelpers.TrimAllowNull(CheckNumber),
            CheckDate = CheckDate,
        };
    }
}
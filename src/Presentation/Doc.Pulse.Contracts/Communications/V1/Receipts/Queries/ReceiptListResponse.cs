using Doc.Pulse.Contracts.Communications.V1.Receipts.Dtos;
using Doc.Pulse.Contracts.Interfaces;

namespace Doc.Pulse.Contracts.Communications.V1.Receipts.Queries;

public class ReceiptListResponse : PaginatedResponseBase
{
    public List<ReceiptListDto> Items { get; set; } = new();
}

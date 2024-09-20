using Doc.Pulse.Contracts.Communications.V1.Vendors.Dtos;
using Doc.Pulse.Contracts.Interfaces;

namespace Doc.Pulse.Contracts.Communications.V1.Vendors.Queries;

public class VendorListResponse : PaginatedResponseBase
{
    public List<VendorListDto> Items { get; set; } = new();
}

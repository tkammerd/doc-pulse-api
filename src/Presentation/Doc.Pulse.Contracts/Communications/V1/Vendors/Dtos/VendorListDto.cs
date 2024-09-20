namespace Doc.Pulse.Contracts.Communications.V1.Vendors.Dtos;

public class VendorListDto
{
    public int? Id { get; set; } = default!;

    public string VendorName { get; set; } = null!;
    public bool Inactive { get; set; } = false;
}
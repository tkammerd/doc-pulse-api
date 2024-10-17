namespace Doc.Pulse.Contracts.Communications.V1.Vendors.Queries;

public class VendorGetByIdResponse
{
    public int? Id { get; set; } = default!;

    public string VendorName { get; set; } = null!;
    public bool Inactive { get; set; } = false;
    public byte[]? RowVersion { get; set; } = null;
}

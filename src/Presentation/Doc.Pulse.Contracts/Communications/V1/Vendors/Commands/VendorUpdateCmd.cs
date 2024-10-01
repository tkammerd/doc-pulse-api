namespace Doc.Pulse.Contracts.Communications.V1.Vendors.Commands;

public class VendorUpdateCmd
{
    public int Id { get; set; }
    public string VendorName { get; set; } = null!;
    public bool Inactive { get; set; } = false;
}
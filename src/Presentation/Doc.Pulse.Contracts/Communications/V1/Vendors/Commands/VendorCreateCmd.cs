namespace Doc.Pulse.Contracts.Communications.V1.Vendors.Commands;

public class VendorCreateCmd
{
    public string VendorName { get; set; } = null!;
    public bool Inactive { get; set; } = false;
}

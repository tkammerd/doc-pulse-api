using Doc.Pulse.Core.Entities;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class VendorDto
{
    public int VendorId { get; set; }
    public string? VendorName { get; set; } = null!;
    public bool Inactive { get; set; } = false;

    public T ToEntity<T>() where T : Vendor, new()
    {
        return new T()
        {
            Id = VendorId,
            VendorName = ParsingHelpers.TrimAllowNull(VendorName),
            Inactive = Inactive
        };
    }
}
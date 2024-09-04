namespace AppDmDoc.SharedKernel.Core.Entities;

public class UserProfile
{
    public string? DisplayName { get; set; } = "John Doe";
    public string? FirstName { get; set; } = "John";
    public string? LastName { get; set; } = "Doe";
    public string? Email { get; set; } = "NOREPLY@la.gov";
    public string? NameIdentifier { get; set; } = "John.Doe";
    public Guid? GuidIdentifier { get; set; } = Guid.Empty;

    public List<string> Roles { get; set; } = [];
    public List<string> IdentityRoles { get; set; } = [];
    public List<string> Claims { get; set; } = [];
}

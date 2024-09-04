using System.Security.Claims;

namespace Doc.Pulse.Core.Entities._Kernel;
public class UserWithClaims
{
    public static readonly UserWithClaims NullUser = new();

    public string Identifier { get; set; } = "[NULL]";
    public string DisplayName { get; set; } = "Unknown";
    public ClaimsPrincipal? ClaimsPrincipal { get; set; }

    private UserWithClaims() { }


    public static UserWithClaims New(ClaimsPrincipal? claimsPrincipal)
    {
        if (claimsPrincipal == null)
        {
            return NullUser;
        }
        else
        {
            return new UserWithClaims()
            {
                Identifier = claimsPrincipal.FindFirst("preferred_username")?.Value ?? "[NULL]",
                DisplayName = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value ?? "Unknown",
                ClaimsPrincipal = claimsPrincipal
            };
        }
    }

    //private ClaimsPrincipal? _claimsPrincipal = default;
    //{
    //    get { return _claimsPrincipal; }
    //    set
    //    {
    //        if (value == null)
    //        {

    //        }
    //        else
    //        {
    //            Identifier = (value!.FindFirst("preferred_username")?.Value ?? "[NULL]");
    //            DisplayName = (value!.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value ?? "Unknown");
    //        }

    //        _claimsPrincipal = value;
    //    }
    //}

    public bool IsNullUser => Identifier == NullUser.Identifier;
}

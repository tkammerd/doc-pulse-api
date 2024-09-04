using Microsoft.AspNetCore.Http;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Entities._Kernel;
using System.Security.Claims;

namespace Doc.Pulse.Tests.Functional._Root.Config;

public class FakeResolveUserService : IResolveUserService
{
    private readonly IHttpContextAccessor _context;

    public FakeResolveUserService(IHttpContextAccessor context)
    {
        _context = context;
    }

    public UserWithClaims GetUserWithClaims()
    {
        var testScheme = "FakeScheme";
        var principal = new ClaimsPrincipal();

        principal.AddIdentity(new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.NameIdentifier, "Serenity"),
            new Claim(ClaimTypes.Name, "wallE"),
            new Claim(ClaimTypes.Role, "Functional Unit Tester"),
            new Claim(ClaimTypes.Role, "DPS-APPDM-Portal-Admin"),
            new Claim("role", "DPS-APPDM-Portal-Admin"),
            new Claim("role", "DPS-APPDM-PORTAL-ADMIN-USERS"),
            new Claim("DocUserDivision", "[\"Public Affairs\", \"Concealed Handguns\"]"),
            new Claim("OtsPermission", "Arc_Access"),
            new Claim("OtsPermission", "Arc_Writer"),
            }, testScheme));

        return UserWithClaims.New(principal);
        //return new TestPrincipal(new Claim("DocUserDivision", SeedData.Categories[0].Name));
    }
}

public class TestPrincipal : ClaimsPrincipal
{
    public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims))
    {
    }
}

public class TestIdentity : ClaimsIdentity
{
    public TestIdentity(params Claim[] claims) : base(claims)
    {
    }
}

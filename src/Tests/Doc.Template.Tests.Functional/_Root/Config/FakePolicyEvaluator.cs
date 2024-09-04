using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Doc.Pulse.Tests.Functional._Root.Config;

public class FakePolicyEvaluator : IPolicyEvaluator
{
    public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
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

        return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal,
            new AuthenticationProperties(), testScheme)));
    }

    public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
        AuthenticateResult authenticationResult, HttpContext context, object? resource)
    {
        return await Task.FromResult(PolicyAuthorizationResult.Success());
    }
}

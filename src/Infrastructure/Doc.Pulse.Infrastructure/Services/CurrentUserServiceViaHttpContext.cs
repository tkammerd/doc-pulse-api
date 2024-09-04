using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Entities;
using AppDmDoc.SharedKernel.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Doc.Pulse.Infrastructure.Services;

public class CurrentUserServiceViaHttpContext : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentUserServiceViaHttpContext(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated { get { return Task.Run(() => GetIsAuthenticated()).Result; } }
    public string DisplayName { get { return Task.Run(() => GetUserDisplayNameAsync()).Result; } }
    public Guid CurrentUserId { get { return Task.Run(() => GetCurrentUserIdAsync()).Result; } }
    public ClaimsPrincipal? User { get { return Task.Run(() => GetCurrentUserClaimsPrincipalAsync()).Result; } }
    public UserProfile UserProfile { get { return Task.Run(() => GetUserProfileAsync()).Result; } }

    public async Task<bool> GetIsAuthenticated() => await Task.FromResult(httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false);

    public async Task<ClaimsPrincipal?> GetCurrentUserClaimsPrincipalAsync() => await Task.FromResult(httpContextAccessor?.HttpContext?.User);

    public async Task<Guid> GetCurrentUserIdAsync()
    {
        var user = httpContextAccessor?.HttpContext?.User;

        if (user == null)
            return Guid.Empty;

        var employeeGuid = user.Claims.Where(x => x.Type.Equals("http://la.gov/ObjectGUID", StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Value.ToGuidFromBase64()).FirstOrDefault();

        return await Task.FromResult(employeeGuid);
    }

    public async Task<string> GetUserDisplayNameAsync()
    {
        var user = httpContextAccessor?.HttpContext?.User;

        if (user == null)
            return "John Doe";

        var firstName = user.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;
        var lastName = user.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            return await GetUserLoginNameAsync();

        return $"{firstName} {lastName}";
    }

    public async Task<string> GetUserLoginNameAsync()
    {
        var user = httpContextAccessor?.HttpContext?.User;

        if (user == null)
            return "John.Doe";

        return await Task.FromResult(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
    }

    public async Task<UserProfile> GetUserProfileAsync()
    {
        var user = httpContextAccessor?.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
            return new UserProfile();

        var firstName = user.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;
        var lastName = user.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;
        var email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        var nameIdentifier = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var profile = new UserProfile()
        {
            DisplayName = $"{firstName} {lastName}",
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            NameIdentifier = nameIdentifier,
            GuidIdentifier = CurrentUserId,

            Roles = user.FindAll(ClaimTypes.Role)?.Select(o => $"{o.Value}").ToList() ?? new(),
            IdentityRoles = user.FindAll("role")?.Select(o => $"{o.Value}").ToList() ?? new(),
            Claims = user.Claims.Select(c => $"[{c.Type}] {c.Value}").ToList()
        };

        return await Task.FromResult(profile);
    }
}

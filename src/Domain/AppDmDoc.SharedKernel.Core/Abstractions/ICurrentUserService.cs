using AppDmDoc.SharedKernel.Core.Entities;
using System.Security.Claims;

namespace AppDmDoc.SharedKernel.Core.Abstractions;

public interface ICurrentUserService
{
    Task<Guid> GetCurrentUserIdAsync();
    Task<string> GetUserDisplayNameAsync();
    Task<string> GetUserLoginNameAsync();

    Task<ClaimsPrincipal?> GetCurrentUserClaimsPrincipalAsync();
    Task<UserProfile> GetUserProfileAsync();

    bool IsAuthenticated { get; }
    string DisplayName { get; }
    Guid CurrentUserId { get; }
    ClaimsPrincipal? User { get; }
    UserProfile UserProfile { get; }
}

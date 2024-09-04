using Microsoft.AspNetCore.Http;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.Infrastructure.Services;

public class ResolveUserServiceByHttpContext : IResolveUserService
{
    private readonly IHttpContextAccessor _context;

    public ResolveUserServiceByHttpContext(IHttpContextAccessor context)
    {
        _context = context;
    }

    public UserWithClaims GetUserWithClaims()
    {
        return UserWithClaims.New(_context.HttpContext?.User);
    }
}

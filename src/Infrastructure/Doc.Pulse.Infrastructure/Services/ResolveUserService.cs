using Microsoft.AspNetCore.Http;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.Infrastructure.Services;

public class ResolveUserServiceByHttpContext(IHttpContextAccessor context) : IResolveUserService
{
    public UserWithClaims GetUserWithClaims()
    {
        return UserWithClaims.New(context.HttpContext?.User);
    }
}

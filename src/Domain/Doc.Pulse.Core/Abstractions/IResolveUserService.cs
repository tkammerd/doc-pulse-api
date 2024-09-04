using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.Core.Abstractions;

public interface IResolveUserService
{
    UserWithClaims GetUserWithClaims();
}

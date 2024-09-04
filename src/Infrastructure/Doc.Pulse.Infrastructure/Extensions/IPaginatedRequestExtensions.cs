using Doc.Pulse.Contracts.Interfaces;
using Doc.Pulse.Core.Config;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class IPaginatedRequestExtensions
{
    public static bool IsPaginated(this IPaginatedRequest query)
    {
        bool IsPaginated = (query.SkipAmount ?? 0) > 0 || query.TakeAmount != Globals.MaxPaginatedPage;

        return IsPaginated;
    }
}

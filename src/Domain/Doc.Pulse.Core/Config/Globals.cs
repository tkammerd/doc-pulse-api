namespace Doc.Pulse.Core.Config;

public static class Globals
{
    public static readonly int MaxPaginatedPage = 999999;

    public static readonly string ApiContextRootSolution = "src/Presentation/Doc.Pulse.Api";   // TODO **** This must be set to the relative solution location for CustomWebApplicationFactory to Work!!!

    public const int UntrackedEntityId = 0;
}

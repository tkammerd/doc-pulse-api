using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

namespace AppDmDoc.SharedKernel.Core.Trouble.Errors;

public class AutomapperFailed : MediatorError
{
    public AutomapperFailed() : base("There was a general mapping issue.") { }

    public static MediatorError New() => new AutomapperFailed();
}

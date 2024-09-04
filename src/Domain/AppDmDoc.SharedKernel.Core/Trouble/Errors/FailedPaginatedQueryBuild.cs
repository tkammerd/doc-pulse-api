using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

namespace AppDmDoc.SharedKernel.Core.Trouble.Errors;

public class FailedPaginatedQueryBuild : MediatorError
{
    public FailedPaginatedQueryBuild() : base($"Something went wrong while constructing the query.") { }

    public static MediatorError New() => new FailedPaginatedQueryBuild();
}

using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

namespace AppDmDoc.SharedKernel.Core.Trouble.Errors;

public class UnexpectedFailure : MediatorError
{
    public UnexpectedFailure() : base("Something unexpected happened.") { }

    public static MediatorError New() => new UnexpectedFailure();
}

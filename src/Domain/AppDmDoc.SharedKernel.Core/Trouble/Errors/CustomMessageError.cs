using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

namespace AppDmDoc.SharedKernel.Core.Trouble.Errors;

public class CustomMessageError : MediatorError
{
    public CustomMessageError(string error) : base(error) { }

    public static MediatorError New(string message) => new CustomMessageError(message);
}
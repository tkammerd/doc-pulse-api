using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

namespace AppDmDoc.SharedKernel.Core.Trouble.Errors;

public class ArgumentNullError : MediatorError
{
    public ArgumentNullError() : base($"Requested Record not Found.", statusCode: System.Net.HttpStatusCode.BadRequest) { }
    public ArgumentNullError(string? argumentName) : base($"Requested Record [{argumentName}] not Found.", statusCode: System.Net.HttpStatusCode.BadRequest) { }

    public static MediatorError New(string? argumentName) => new ArgumentNullError(argumentName);
}

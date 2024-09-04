using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

namespace AppDmDoc.SharedKernel.Core.Trouble.Errors;

public class HashIdNotValid : MediatorError
{
    public HashIdNotValid() : base($"The Identifier Specified is NOT Valid, but if at First You Don't Succeed...", statusCode: System.Net.HttpStatusCode.BadRequest) { }
    public HashIdNotValid(string? hashId) : base($"The Identifier Specified [{hashId}] is NOT Valid, but if at First You Don't Succeed...", statusCode: System.Net.HttpStatusCode.BadRequest) { }

    public static MediatorError New(string? identifier) => new HashIdNotValid(identifier);
}

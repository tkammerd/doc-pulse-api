using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

namespace AppDmDoc.SharedKernel.Core.Trouble.Errors;

public class EntityNotFound : MediatorError
{
    public EntityNotFound() : base($"Requested Record not Found.", statusCode: System.Net.HttpStatusCode.BadRequest) { }
    public EntityNotFound(string? identifier) : base($"Requested Record [{identifier}] not Found.", statusCode: System.Net.HttpStatusCode.BadRequest) { }

    public static MediatorError New(string? identifier) => new EntityNotFound(identifier);
}

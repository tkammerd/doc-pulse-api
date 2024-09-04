using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

namespace AppDmDoc.SharedKernel.Core.Trouble.Errors;

public class NullDtoAfterMapping : MediatorError
{
    public NullDtoAfterMapping() : base($"Mapping Data from Database to Response Failed.") { }

    public static MediatorError New() => new NullDtoAfterMapping();
}

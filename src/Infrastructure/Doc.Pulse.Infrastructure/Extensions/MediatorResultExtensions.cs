using AutoMapper;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class MediatorResultExtensions
{
    public static MediatorResult<T> WithException<T>(this MediatorResult<T> result, Exception exception)
    {
        if (exception is AutoMapperMappingException ammException)
        {
            var error = AutomapperFailed.New().CausedBy(ammException);

            return result.WithError(error);
        }
        //else if (exception is NoResultException nrException)
        else if (exception is MediatorException aeException)
        {
            var error = CustomMessageError.New(aeException.Message).CausedBy(aeException);

            return result.WithError(error);
        }
        else
        {
            var error = UnexpectedFailure.New().CausedBy(exception);

            return result.WithError(error);
        }
    }
}

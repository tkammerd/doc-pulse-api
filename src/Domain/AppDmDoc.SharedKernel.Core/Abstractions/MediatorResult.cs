using FluentResults;
using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;
using AppDmDoc.SharedKernel.Core.Entities;
using Doc.Pulse.Core.Abstractions;
using System.Diagnostics;

namespace AppDmDoc.SharedKernel.Core.Abstractions;

public class MediatorResult<T> : Result<T>, IMediatorResult
{
    //public new MediatRResponse<T> WithValue(T value) => base.WithValue(value);

    public new MediatorResult<T> WithValue(T value)
    {
        Result.Ok(value);

        base.WithValue(value);

        return this as MediatorResult<T> ?? throw new UnreachableException("Something terrible went wrong while processing a MediatRResponse - basically class cast exception.");
    }

    public new MediatorResult<T> WithError(IError error)
    {
        base.WithError(error);

        return this as MediatorResult<T> ?? throw new UnreachableException("Something terrible went wrong while processing a MediatRResponse - basically class cast exception.");
    }

    public TResponse WithError<TResponse>(IError error) where TResponse : class, IMediatorResult //MediatorResult<?>
    {
        base.WithError(error);

        return this as TResponse ?? throw new UnreachableException("Something terrible went wrong while processing a MediatRResponse - basically class cast exception.");
    }

    public TResponse WithError<TResponse, TMediatorError>(params string[] args) 
        where TResponse : class, IMediatorResult
        where TMediatorError : MediatorError, new()
    {
        if (!args.Any())
        {
            base.WithError(new TMediatorError());
        }
        else
        {
            var instance = Activator.CreateInstance(typeof(TMediatorError), args) as TMediatorError;

            base.WithError(instance);
        }

        return this as TResponse ?? throw new UnreachableException("Something terrible went wrong while processing a MediatRResponse - basically class cast exception.");
    }

    public TResponse WithValue<TResponse>(T value) where TResponse : MediatorResult<T>
    {
        base.WithValue(value);

        return this as TResponse ?? throw new UnreachableException("Something terrible went wrong while processing a MediatRResponse - basically class cast exception.");
    }

    //public void HandleExpectedExceptions(Exception exception)
    //{
    //    if (exception is AutoMapperMappingException ammException)
    //    {
    //        var error = new AutomapperFailed();
    //        error.CausedBy(ammException);

    //        WithError(error);
    //    }
    //    //else if (exception is NoResultException nrException)
    //    else if (exception is MediatRExceptionBase aeException)
    //    {
    //        var error = new CustomMessageError(aeException.Message);
    //        error.CausedBy(aeException);

    //        WithError(error);
    //    }
    //    else
    //    {
    //        var error = new UnexpectedFailure();
    //        error.CausedBy(exception);

    //        WithError(error);
    //    }
    //}

    public static MediatorResult<T> Fail(IError error) => Fail<T>(error);
    public static MediatorResult<T> Ok(T value) => Ok<T>(value);

    public static MediatorResult<TValue> Ok<TValue>(TValue value)
    {
        MediatorResult<TValue> result = new();
        result.WithValue(value);
        return result;
    }

    public static MediatorResult<TValue> Fail<TValue>(IError error)
    {
        MediatorResult<TValue> result = new();
        result.WithError(error);
        return result;
    }

    public static MediatorResult<TValue> Fail<TValue>(Exception exception) => Fail<TValue>(new ResultException(exception));
}

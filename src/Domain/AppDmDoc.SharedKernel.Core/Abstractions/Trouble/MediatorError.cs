using FluentResults;
using System.Net;

namespace AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

public abstract class MediatorError : Error
{
    public MediatorError(string message, string errorCode = MediatorException.DefaultErrorCode, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
    {
        WithMetadata("HttpStatusCode", statusCode);
        WithMetadata("ErrorCode", errorCode);
    }

    public HttpStatusCode HttpStatusCode => Metadata["HttpStatusCode"] as HttpStatusCode? ?? HttpStatusCode.InternalServerError;
    public string ErrorCode => Metadata["ErrorCode"] as string ?? MediatorException.DefaultErrorCode;

    //public TResponse AsResult<TResponse, T>(MediatorResult<T> result)
    //    where TResponse : MediatorResult<T>
    //{
    //    return (TResponse) result.WithError(this);
    //}

    //public static MediatorError New<T>() where T : MediatorError, new()
    //{
        
    //}

    //public static MediatorError NewAutomapperFailed() => AutomapperFailed.New();
    //public static MediatorError NewCustomMessageError(string message) => CustomMessageError.New(message);
    //public static MediatorError NewDatabaseSaveFailed() => DatabaseSaveFailed.New();


    //public static MediatorError NewFailedPaginatedQueryBuildError() => FailedPaginatedQueryBuild.New();

    


    //public TResponse WithError<TResponse>(IError error) where TResponse : class, IMediatorResult //MediatorResult<?>
    //{
    //    base.WithError(error);

    //    return this as TResponse ?? throw new UnreachableException("Something terrible went wrong while processing a MediatRResponse - basically class cast exception.");
    //}
}

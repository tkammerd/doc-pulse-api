using AppDmDoc.SharedKernel.Core.Entities;
using Doc.Pulse.Core.Abstractions;

namespace Doc.Pulse.Core.Entities._Kernel;

public class MediatrResultBase : FluentResults.Result, IMediatorResult
{

}

public class MediatrResultBase<T> : FluentResults.Result<T>, IMediatrResult<T>
{

    public static MediatrResultBase<TValue> Fail<TValue>(IResultError error)
    {
        MediatrResultBase<TValue> result = new();
        result.WithError(error);
        return result;
    }

    public static MediatrResultBase<TValue> Ok<TValue>(TValue value)
    {
        MediatrResultBase<TValue> result = new();
        result.WithValue(value);
        return result;
    }

    public static MediatrResultBase<TValue> Fail<TValue>(Exception exception) => MediatrResultBase<TValue>.Fail<TValue>(new ResultException(exception));
}

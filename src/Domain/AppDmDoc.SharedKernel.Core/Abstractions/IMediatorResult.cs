using FluentResults;

namespace AppDmDoc.SharedKernel.Core.Abstractions;

public interface IMediatorResult : IResultBase
{
}

public interface IMediatrResult<out TValue> : IResult<TValue>
{
}

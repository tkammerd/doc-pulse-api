using FluentResults;

namespace Doc.Pulse.Core.Abstractions;

public interface IMediatorResult : IResultBase
{
}

public interface IMediatrResult<out TValue> : IResult<TValue>
{
}

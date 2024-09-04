using FluentResults;

namespace AppDmDoc.SharedKernel.Core.Entities;

public class ResultException : ResultError, IExceptionalError
{
    public Exception Exception { get; }

    public ResultException(Exception exception) : this(exception.Message, exception)
    {
    }

    public ResultException(string message, Exception exception) : base(message)
    {
        Exception = exception;
    }
}

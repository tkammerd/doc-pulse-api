using FluentResults;
using Doc.Pulse.Core.Abstractions;
using System.Net;

namespace AppDmDoc.SharedKernel.Core.Entities;

public class ResultError : Error, IResultError
{
    public HttpStatusCode StatusCode { get; init; }

    //public new Dictionary<string, object> Metadata { get; }
    public new List<IResultError> Reasons { get; }

    protected ResultError() : base()
    {
        //Metadata = new Dictionary<string, object>();
        Reasons = new List<IResultError>();
    }
    public ResultError(string message) : this()
    {
        Message = message;
    }
    public ResultError(string message, IResultError innerError) : this(message)
    {
        if (innerError == null)
        {
            throw new ArgumentNullException(nameof(innerError));
        }

        Reasons.Add(innerError);
    }

    public ResultError(HttpStatusCode statusCode, string message, IResultError innerError) : this(message, innerError)
    {
        StatusCode = statusCode;
    }
    public ResultError(HttpStatusCode statusCode, string message) : this(message)
    {
        StatusCode = statusCode;
    }
}

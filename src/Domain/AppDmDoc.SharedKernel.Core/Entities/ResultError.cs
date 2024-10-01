using AppDmDoc.SharedKernel.Core.Abstractions;
using FluentResults;
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
        Reasons = [];
    }
    public ResultError(string message) : this()
    {
        Message = message;
    }
    public ResultError(string message, IResultError innerError) : this(message)
    {
        ArgumentNullException.ThrowIfNull(innerError);

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

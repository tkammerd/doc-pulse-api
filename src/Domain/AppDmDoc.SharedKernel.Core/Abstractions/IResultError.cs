using FluentResults;
using System.Net;

namespace Doc.Pulse.Core.Abstractions;

public interface IResultError : IError
{
    HttpStatusCode StatusCode { get; }
    //string Message { get; }
    //IError? InnerException { get; }
}

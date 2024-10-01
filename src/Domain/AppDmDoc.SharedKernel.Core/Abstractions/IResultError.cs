using FluentResults;
using System.Net;

namespace AppDmDoc.SharedKernel.Core.Abstractions;

public interface IResultError : IError
{
    HttpStatusCode StatusCode { get; }
    //string Message { get; }
    //IError? InnerException { get; }
}

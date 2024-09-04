using System.Net;

namespace AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

public class MediatorException : Exception
{
    public const string DefaultErrorCode = "0042";

    public HttpStatusCode HttpStatusCode { get; init; } = HttpStatusCode.BadRequest;
    public string ErrorCode { get; init; } = DefaultErrorCode;
    //public string Messsage { get; set; }


    //public MediatRError Error { get; set; }

    private MediatorException(string? message) : base(message) { }

    private MediatorException(HttpStatusCode httpStatusCode, string errorCode, string? message) : base(message)
    {
        HttpStatusCode = httpStatusCode;
        ErrorCode = errorCode;
    }

    private MediatorException(HttpStatusCode httpStatusCode, string errorCode, string? message, Exception? innerException) : base(message, innerException)
    {
        HttpStatusCode = httpStatusCode;
        ErrorCode = errorCode;
    }

    public static MediatorException New(string? message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, string errorCode = DefaultErrorCode, Exception? innerException = null) 
    {
        return new MediatorException(httpStatusCode, errorCode, message, innerException);
    }
}

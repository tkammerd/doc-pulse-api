using AppDmDoc.SharedKernel.Core.Abstractions;
using System.Net;

namespace Doc.Pulse.Core.Entities._Kernel;

public class ApiResponse
{
    public Guid ResponseUid { get; set; } = Guid.Empty;
    public string Version { get; init; } = string.Empty;
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

    public bool? IsSuccess { get; set; } = default;
    public string Message { get; set; } = "Result Undetermined.";//"Operation was Successful.";
    public IEnumerable<ApiError>? Errors { get; set; }
}

public class ApiResponse<TContract> : ApiResponse
{
    public TContract? Result { get; set; } = default;
    public int ErrorID { get; internal set; }

    public ApiResponse() { }
    public ApiResponse(HttpStatusCode? statusCode = HttpStatusCode.OK, TContract? value = default) : this()
    {
        StatusCode = statusCode ?? HttpStatusCode.OK;
        Result = value;
    }

    public ApiResponse(HttpStatusCode statusCode) : this(statusCode, default) { }
    public ApiResponse(TContract value) : this(default, value) { }
}

public static class ApiResponseFactory
{
    private static readonly List<HttpStatusCode> _isSuccessfulCodes = [
        HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.NoContent 
    ];

    private const string _default_Success = "Operation was successful.";
    private const string _default_Failure = "Apologies - something unexpected has gone wrong. Please contact the help desk if it persists.";
    private const string _default_FailureController = "Something Terrible has Happened and The Request Could Not be Routed.";

    public static ApiResponse<T> Ok<T>(T value, string? message = default, OtsApiVersion? version = null)
    {
        return new ApiResponse<T>(value)
        {
            ResponseUid = Guid.NewGuid(),
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Message = message ?? _default_Success,
            Version = version?.Display ?? OtsApiVersion.Default.Display,
        };
    }

    public static ApiResponse<string> Fail(HttpStatusCode statusCode, string? message = default, IEnumerable<ApiError>? errors = default, OtsApiVersion? version = null)
    {
        

        var resp = new ApiResponse<string>(statusCode)
        {
            ResponseUid = Guid.NewGuid(),
            Message = message ?? _default_Failure,
            IsSuccess = false,
            Errors = errors ?? default,
            Version = version?.Display ?? OtsApiVersion.Default.Display,
        };

        return resp;
    }

    public static ApiResponse<string> ControllerFail(Exception exception, int databaseLogID, Guid responseUid, string? message = default, OtsApiVersion? version = null)
    {
       
        var resp = new ApiResponse<string>(HttpStatusCode.InternalServerError)
        {
            ResponseUid = responseUid,
            Message = message ?? _default_FailureController,
            IsSuccess = false,
            Version = version?.Display ?? OtsApiVersion.Default.Display,
            Errors = GetRecordFromException(exception),
            ErrorID = databaseLogID
        };

        return resp;
    }

    public static IList<ApiError> GetRecordFromException(Exception e)
    {
        

        return
        [
            new ApiError(e.Message, e.StackTrace)
        ];
    }
}

public class OtsApiVersion : ValueObject
{
    public string Name { get; set; } = default!;

    public string Display => $"v{Name}";

    private OtsApiVersion(string name) { Name = name; }

    public static readonly OtsApiVersion Default = New("1.0");
    public static readonly OtsApiVersion v1_0 = Default;
    public static readonly OtsApiVersion v1_1 = New("1.1");

    public static OtsApiVersion New(string versionName)
    {
        ArgumentNullException.ThrowIfNull(versionName, nameof(versionName));

        return new OtsApiVersion(versionName);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}
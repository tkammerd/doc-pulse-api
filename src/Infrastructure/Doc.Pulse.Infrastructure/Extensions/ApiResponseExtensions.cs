using Microsoft.AspNetCore.Mvc;
using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class ApiResponseExtensions
{
    public static ObjectResult ToObjectResult(this ApiResponse apiResult)
    {
        return new ObjectResult(apiResult) { StatusCode = (int)apiResult.StatusCode };
    }
}

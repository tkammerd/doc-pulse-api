using FluentResults;
using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;
using Microsoft.AspNetCore.Mvc;
using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.Api.Helpers;

public static class ActionResultExtensions
{
    //public static IActionResult ToActionResult(this BadRequestObjectResult result)
    //{
    //    var apiResp = ApiResponseFactory.ControllerFail(message: result.Value?.ToString() ?? "Something Unexpceted Went Wrong.");

    //    return new ObjectResult(apiResp) { StatusCode = (int)apiResp.StatusCode };
    //}

    public static IActionResult ToActionResult<TResult>(this Result<TResult> result, OtsApiVersion? apiVersion = null)
    {
        //ResultException rsltException = null;

        if (result == null)
            return new BadRequestObjectResult(new { Message = "Something Unexpected Went Wrong." });
        else if (result.IsSuccess == true)
        {
            var rslt = new OkObjectResult(ApiResponseFactory.Ok(result.Value, version:apiVersion));

            return rslt;
        }
        //    return new OkObjectResult(result.Value);
        //else if (result.HasException<ResultException>(exception => { rsltException = exception; return true; }, out var exceptions))
        //    return rsltException!.ToStatusCodeRespose();

        else if (result.IsFailed)
        {
            result.HasError<MediatorError>(o => o.HasMetadataKey("HttpStatusCode"), out var errors);

            if (errors?.Any() == true)
            {
                var apiErrors = errors.Select(err => new ApiError(err.ErrorCode, err.Message));

                if (errors.Count() == 1)
                {
                    var error = errors.First();

                    var apiResp = ApiResponseFactory.Fail(error.HttpStatusCode, error.Message, apiErrors, apiVersion);
                    return new ObjectResult(apiResp) { StatusCode = (int)apiResp.StatusCode };
                }
                else
                {
                    var error = errors.First();

                    var apiResp = ApiResponseFactory.Fail(error.HttpStatusCode, "Multiple Errors Occurred.", apiErrors, apiVersion);
                    return new ObjectResult(apiResp) { StatusCode = (int)apiResp.StatusCode };
                }
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        else
            return new BadRequestObjectResult(new { Message = "Something Unexpected Went Wrong." });
    }
}


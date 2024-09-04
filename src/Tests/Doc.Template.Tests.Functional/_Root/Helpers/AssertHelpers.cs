using Doc.Pulse.Core.Entities._Kernel;
using System.Net;
using System.Text;

namespace Doc.Pulse.Tests.Functional._Root.Helpers;
internal static class AssertHelpers
{
    public static TDto AssertApiResponseSuccess<TDto>(this ApiResponse<TDto>? response) where TDto : class
    {
        Assert.NotNull(response);

        if (response?.IsSuccess == false)
        {
            StringBuilder sb = new();
            sb.AppendLine(response.Message);
            if (response?.Errors?.Any() == true)
            {
                foreach (ApiError err in response.Errors)
                {
                    sb.AppendLine($"* [{err.Code}] {err.Description}");
                }
            }

            Assert.Fail(sb.ToString());
        }

        Assert.NotNull(response?.Result);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccess);

        TDto result = Assert.IsAssignableFrom<TDto>(response.Result);
        return result;
    }

    //public static ApiResponse<TDto> AssertQuerySuccess<TDto>(this ApiResponse<TDto>? response) where TDto : class
    //{
    //    Assert.NotNull(response);

    //    if (response?.IsSuccess == false)
    //    {
    //        StringBuilder sb = new();
    //        sb.AppendLine(response.Message);
    //        if (response?.Errors?.Any() == true)
    //        {
    //            foreach (ApiError err in response.Errors)
    //            {
    //                sb.AppendLine($"* [{err.Code}] {err.Description}");
    //            }
    //        }

    //        Assert.Fail(sb.ToString());
    //    }

    //    Assert.NotNull(response?.Result);
    //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //    Assert.True(response.IsSuccess);

    //    return response;
    //}

    public static (string, IEnumerable<ApiError>) AssertResponseBadRequest<TDto>(this ApiResponse<TDto>? response) where TDto : class
    {
        Assert.NotNull(response);
        Assert.NotNull(response.Errors);
        Assert.NotNull(response.Message);
        Assert.NotEqual(string.Empty, response.Message);

        Assert.NotEmpty(response.Errors);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.False(response.IsSuccess);

        return (response.Message, response.Errors ?? Enumerable.Empty<ApiError>());
    }
}

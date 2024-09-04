using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Doc.Pulse.Core.Attributes;
using OtsLogger;
using OtsLogger.Configuration;
using System.Diagnostics;

namespace Doc.Pulse.Infrastructure.Util;

public class DefaultDocMediatrLogging<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<DefaultDocMediatrLogging<TRequest, TResponse>> _logger;
    private readonly IOptions<OtsLoggerOptions> _options;

    public DefaultDocMediatrLogging(ILogger<DefaultDocMediatrLogging<TRequest, TResponse>> logger, IOptions<OtsLoggerOptions> options)
    {
        _logger = logger;
        _options = options;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        bool skipLogging = Attribute.GetCustomAttribute(typeof(TRequest), typeof(PreventMediatorLoggingAttribute)) != null;

        if (skipLogging) await next();
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        string requestName = GetRequestTypeName(typeof(TRequest));
        var watch = Stopwatch.StartNew();

        try
        {
            //var reqClone = request.TryTrimProperties(200, 50, false) ?? request;

            _logger.LogDebug("MediatR Start Handling: {RequestName}. Request: {@Request}", requestName, request);

            var response = await next();
            watch.Stop();

            _logger.Log(_options.Value.DefaultMediatrMiddlewareLoggingLevel, "Successfully processed {RequestName} in {Elapsed} ms. Response: {@Response}", requestName, watch.Elapsed.TotalMilliseconds, response);

            //var respClone = response.TryTrimProperties(200, 50, false) ?? response;

            //Result<object>? wrapper = default;
            //try
            //{
            //    wrapper = (Result<object>)response!;
            //}
            //catch (Exception) { }

            ////if (response?.TryCast(out Result<object> wrapper) == true)
            //if (wrapper.HasValue)
            //{
            //    wrapper.Value.IfSucc(o =>
            //    {
            //        _logger.Log(_options.Value.DefaultMediatrMiddlewareLoggingLevel, "Successfully processed {RequestName} in {Elapsed} ms. Response: {@Response}", requestName, watch.Elapsed.TotalMilliseconds, o);
            //    });

            //    wrapper.Value.IfFail(o =>
            //    {
            //        _logger.LogWarning("Failed to process {RequestName} in {Elapsed} ms. Response: {@Response}", requestName, watch.Elapsed.TotalMilliseconds, o);
            //    });
            //}
            //else
            //{
            //    _logger.Log(_options.Value.DefaultMediatrMiddlewareLoggingLevel, "Successfully processed {RequestName} in {Elapsed} ms. Response: {@Response}", requestName, watch.Elapsed.TotalMilliseconds, response.ToSome());
            //}

            return response;
        }
        catch (Exception ex)
        {
            watch.Stop();
            using (LogContextBuilder.PushException(ex))
            {
                _logger.LogError("Failed processing {RequestName} in {Elapsed} ms with message {ErrorMessage}", requestName, watch.Elapsed.TotalMilliseconds, ex.Message);
            }

            throw;
        }
    }


    private string GetRequestTypeName(Type requestType)
    {
        string requestName = requestType.Name;

        try
        {
            requestName = string.Join(".", (requestType.FullName ?? "").Split('.').Reverse().Take(2));
        }
        catch (Exception) { }

        return requestName;
    }
}
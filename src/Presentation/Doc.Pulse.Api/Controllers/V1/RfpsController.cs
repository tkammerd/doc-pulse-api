﻿using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;
using Doc.Pulse.Api.Features.Rfps.Commands;
using Doc.Pulse.Api.Features.Rfps.Queries;
using Doc.Pulse.Api.Helpers;
using Doc.Pulse.Contracts.Communications.V1.Rfps.Commands;
using Doc.Pulse.Contracts.Communications.V1.Rfps.Queries;
using Doc.Pulse.Core.Entities._Kernel;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Doc.Pulse.Api.Controllers.V1;

[ApiVersion("1.0")]
public class RfpsController(ILogger<RfpsController> logger, IMediator mediator) : BaseApiController(mediator)
{
    private readonly ILogger<RfpsController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var result = await _mediator.Send(new RfpListHandler.Request()
        {
            Query = new RfpListQry()
        });
        return result.ToActionResult();
    }

    /// <summary>
    /// Get Paginated List of Rfps
    /// </summary>
    /// <param name="query">Id property within RfpGetByIdQry query</param>
    /// <remarks>
    ///     Specify the parameters for your list with JSON that includes:
    ///     <ul>
    ///         <li>SkipAmount: The number of list items to skip before the first displayed list item</li>
    ///         <li>TakeAmount: The number of list items to display after the skip</li>
    ///         <li>SortBy:      The name of a property (case sensitive) on which to sort the entire list</li>
    ///         <li>
    ///             Filter:      The name of a property and the initial characters of its value to filter on, 
    ///             in JSON format i.e. {"propertyName":"initialChars"}
    ///         </li>
    ///         <li>SortDesc:    Select True to sort descending, False to sort ascending, -- for default sort direction</li>
    ///     </ul>
    /// </remarks>
    /// <returns>A list of Rfp values</returns>
    [HttpGet]
    public async Task<IActionResult> ListPaginated([FromQuery] RfpListQry query)
    {
        var result = await _mediator.Send(new RfpPaginatedListHandler.Request() { Query = query });
        return result.ToActionResult();
    }


#pragma warning disable CS1572  // XML comment has a param tag, but there is no parameter by that name
#pragma warning disable CS1573  // Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning disable ASP0018 // Unused route parameter
                                // Justification: Swagger finds param "Id" as a property of param "query"
    /// <summary>
    /// Get Simple Rfp
    /// </summary>
    /// <param name="Id">Value of RfpId to get</param>
    /// <paramref name="query">RfpGetByIdQry parameter containing Id property</paramref>
    /// <remarks>
    /// The property specified in the route will be automatically found within the method parameters if their names do not match.
    /// In this endpoint, <i>Id</i> will be found within the RfpGetByIdQry parameter <i>query</i>.<br/>
    /// The value returned is in a the default response wrapper with extensions.
    /// </remarks>
    /// <returns>The value of the Rfp with the specified Id</returns>
    [HttpGet("{Id}", Name = "Get Simple Rfp by id")]
#pragma warning restore CS1572  // XML comment has a param tag, but there is no parameter by that name
#pragma warning restore ASP0018 // Unused route parameter
    public async Task<IActionResult> GetById([FromRoute] RfpGetByIdQry query)
#pragma warning restore CS1573  // Parameter has no matching param tag in the XML comment (but other parameters do)
    {
        var result = await _mediator.Send(new RfpGetByIdHandler.Request() { Query = query });
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RfpCreateCmd command)
    {
        var result = await _mediator.Send(new RfpCreateHandler.Request() { Command = command });
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> HardDelete([FromBody] RfpHardDeleteCmd command)
    {
        var result = await _mediator.Send(new RfpHardDeleteHandler.Request() { Command = command });
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] RfpUpdateCmd command)
    {
        var result = await _mediator.Send(new RfpUpdateHandler.Request() { Command = command });
        return result.ToActionResult();
    }


#pragma warning disable CS1572  // XML comment has a param tag, but there is no parameter by that name
#pragma warning disable CS1573  // Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning disable ASP0018 // Unused route parameter
                                // Justification: Swagger finds param "Id" as a property of param "query"
    /// <summary>
    /// Get Rfp without extensions
    /// </summary>
    /// <param name="Id">Value of RfpId to get</param>
    /// <paramref name="query">RfpGetByIdQry parameter containing Id property</paramref>
    /// <remarks>
    /// The property specified in the route will be automatically found within the method parameters if their names do not match.
    /// In this endpoint, <i>Id</i> will be found within the RfpGetByIdQry parameter <i>query</i>.<br/>
    /// The value returned is in a simpler customized response wrapper.
    /// </remarks>
    /// <returns>The value of the Rfp with the specified Id</returns>
    [HttpGet("raw/{Id}", Name = "Get Rfp by id without extensions")]
#pragma warning restore CS1572  // XML comment has a param tag, but there is no parameter by that name
#pragma warning restore ASP0018 // Unused route parameter
    public async Task<IActionResult> GetById_WithoutExtensions([FromRoute] RfpGetByIdQry query)
#pragma warning restore CS1573  // Parameter has no matching param tag in the XML comment (but other parameters do)
    {
        try
        {
            var result = await _mediator.Send(new RfpGetByIdHandler.Request() { Query = query });

            if (result?.IsSuccess == true)
            {
                var response = new ApiResponse<RfpGetByIdResponse>()
                {
                    ResponseUid = Guid.NewGuid(),
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = "Operation was successful.",
                    Result = result.Value
                };

                return Ok(response);
            }
            else if (result?.IsFailed == true)
            {
                result.HasError<MediatorError>(o => o.HasMetadataKey("HttpStatusCode"), out var errors);

                if (errors?.Any() == true)
                {
                    var error = errors.First();
                    var message = (errors.Count() == 1) ? error.Message : "Multiple Errors Occurred.";
                    var apiErrors = errors.Select(err => new ApiError(err.ErrorCode, err.Message));

                    var apiResp = ApiResponseFactory.Fail(error.HttpStatusCode, message, apiErrors);
                    var response = new ApiResponse<string>()
                    {
                        ResponseUid = Guid.NewGuid(),
                        StatusCode = error.HttpStatusCode,
                        Message = message ?? "Apologies - something unexpected has gone wrong. Please contact the help desk if it persists.",
                        IsSuccess = false,
                        Errors = apiErrors
                    };

                    return StatusCode((int)error.HttpStatusCode, response);
                }
            }
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<string>(HttpStatusCode.InternalServerError)
            {
                ResponseUid = Guid.NewGuid(),
                Message = "Something Terrible has Happened and The Request Could Not be Routed.",
                IsSuccess = false,
            };
            _logger.LogError(ex, "({ResponseUid}) {Message}", response.ResponseUid, response.Message);

            return StatusCode((int)HttpStatusCode.InternalServerError, response);
        }

        return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "Something Unexpected Went Wrong." });
    }
}
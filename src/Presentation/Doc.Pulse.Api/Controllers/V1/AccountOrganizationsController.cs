﻿using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;
using Doc.Pulse.Api.Features.AccountOrganizations.Commands;
using Doc.Pulse.Api.Features.AccountOrganizations.Queries;
using Doc.Pulse.Api.Helpers;
using Doc.Pulse.Contracts.Communications.V1.AccountOrganizations.Commands;
using Doc.Pulse.Contracts.Communications.V1.AccountOrganizations.Queries;
using Doc.Pulse.Core.Entities._Kernel;
using Doc.Pulse.Infrastructure.Abstractions;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Doc.Pulse.Api.Controllers.V1;

[ApiVersion("1.0")]
public class AccountOrganizationsController(ILogger<AccountOrganizationsController> logger, IMediator mediator) : BaseApiController(mediator)
{
    private readonly ILogger<AccountOrganizationsController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var result = await _mediator.Send(new AccountOrganizationListHandler.Request()
        {
            Query = new AccountOrganizationListQry()
        });
        return result.ToActionResult();
    }

    /// <summary>
    /// Get Paginated List of AccountOrganizations
    /// </summary>
    /// <param name="query">Id property within AccountOrganizationGetByIdQry query</param>
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
    /// <returns>A list of AccountOrganization values</returns>
    [HttpGet]
    public async Task<IActionResult> ListPaginated([FromQuery] AccountOrganizationListQry query)
    {
        var result = await _mediator.Send(new AccountOrganizationPaginatedListHandler.Request() { Query = query });
        return result.ToActionResult();
    }


#pragma warning disable CS1572  // XML comment has a param tag, but there is no parameter by that name
#pragma warning disable CS1573  // Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning disable ASP0018 // Unused route parameter
                                // Justification: Swagger finds param "Id" as a property of param "query"
    /// <summary>
    /// Get Simple AccountOrganization
    /// </summary>
    /// <param name="Id">Value of AccountOrganizationId to get</param>
    /// <paramref name="query">AccountOrganizationGetByIdQry parameter containing Id property</paramref>
    /// <remarks>
    /// The property specified in the route will be automatically found within the method parameters if their names do not match.
    /// In this endpoint, <i>Id</i> will be found within the AccountOrganizationGetByIdQry parameter <i>query</i>.<br/>
    /// The value returned is in a the default response wrapper with extensions.
    /// </remarks>
    /// <returns>The value of the AccountOrganization with the specified Id</returns>
    [HttpGet("{Id}", Name = "Get Simple AccountOrganization by id")]
#pragma warning restore CS1572  // XML comment has a param tag, but there is no parameter by that name
#pragma warning restore ASP0018 // Unused route parameter
    public async Task<IActionResult> GetById([FromRoute] AccountOrganizationGetByIdQry query)
#pragma warning restore CS1573  // Parameter has no matching param tag in the XML comment (but other parameters do)
    {
        var result = await _mediator.Send(new AccountOrganizationGetByIdHandler.Request() { Query = query });
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AccountOrganizationCreateCmd command)
    {
        var result = await _mediator.Send(new AccountOrganizationCreateHandler.Request() { Command = command });
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> HardDelete([FromBody] AccountOrganizationHardDeleteCmd command)
    {
        var result = await _mediator.Send(new AccountOrganizationHardDeleteHandler.Request() { Command = command });
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] AccountOrganizationUpdateCmd command)
    {
        var result = await _mediator.Send(new AccountOrganizationUpdateHandler.Request() { Command = command });
        return result.ToActionResult();
    }


#pragma warning disable CS1572  // XML comment has a param tag, but there is no parameter by that name
#pragma warning disable CS1573  // Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning disable ASP0018 // Unused route parameter
                                // Justification: Swagger finds param "Id" as a property of param "query"
    /// <summary>
    /// Get AccountOrganization without extensions
    /// </summary>
    /// <param name="Id">Value of AccountOrganizationId to get</param>
    /// <paramref name="query">AccountOrganizationGetByIdQry parameter containing Id property</paramref>
    /// <remarks>
    /// The property specified in the route will be automatically found within the method parameters if their names do not match.
    /// In this endpoint, <i>Id</i> will be found within the AccountOrganizationGetByIdQry parameter <i>query</i>.<br/>
    /// The value returned is in a simpler customized response wrapper.
    /// </remarks>
    /// <returns>The value of the AccountOrganization with the specified Id</returns>
    [HttpGet("raw/{Id}", Name = "Get AccountOrganization by id without extensions")]
#pragma warning restore CS1572  // XML comment has a param tag, but there is no parameter by that name
#pragma warning restore ASP0018 // Unused route parameter
    public async Task<IActionResult> GetById_WithoutExtensions([FromRoute] AccountOrganizationGetByIdQry query)
#pragma warning restore CS1573  // Parameter has no matching param tag in the XML comment (but other parameters do)
    {
        try
        {
            var result = await _mediator.Send(new AccountOrganizationGetByIdHandler.Request() { Query = query });

            if (result?.IsSuccess == true)
            {
                var response = new ApiResponse<AccountOrganizationGetByIdResponse>()
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
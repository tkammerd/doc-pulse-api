using AutoMapper;
using AppDmDoc.SharedKernel.Core.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.AccountOrganizations.Dtos;
using Doc.Pulse.Contracts.Communications.V1.AccountOrganizations.Queries;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.AccountOrganizations.Queries;

public class AccountOrganizationListHandler
{
    public class Request : IRequest<Response>
    {
        public required AccountOrganizationListQry Query { get; set; }
    }
    public class Response : MediatorResult<AccountOrganizationListResponse> { }


    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AccountOrganization, AccountOrganizationListDto>();
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entities = await _dbContext.AccountOrganizations
                    .OrderBy(o => o.AccountOrganizationNumber)
                    .ToListAsync(cancellationToken);

                var dtos = _mapper.Map<List<AccountOrganizationListDto>>(entities);

                response.WithValue<Response>(new AccountOrganizationListResponse()
                {
                    Items = dtos,
                    CountAvailable = dtos.Count,
                    CountTotal = dtos.Count
                });
            }
            catch (Exception exception)
            {
                response.WithException(exception);
            }

            return response;
        }
    }
}

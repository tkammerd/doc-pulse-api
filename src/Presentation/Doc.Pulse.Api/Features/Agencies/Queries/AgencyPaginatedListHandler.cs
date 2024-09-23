using AutoMapper;
using FluentValidation;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.Agencies.Dtos;
using Doc.Pulse.Contracts.Communications.V1.Agencies.Queries;
using Doc.Pulse.Core.Config;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Api.Features.Agencies.Queries;

public partial class AgencyPaginatedListHandler
{
    public class Request : IRequest<Response>
    {
        public required AgencyListQry Query { get; set; }
    }
    public class Response : MediatorResult<AgencyListResponse> { }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Agency, AgencyListDto>();
        }
    }

    public class Validator : AbstractValidator<Contracts.Bases.DefaultQueryContractsBase.PaginatedList>
    {
        public Validator()
        {
            RuleFor(o => o.TakeAmount).InclusiveBetween(0, Globals.MaxPaginatedPage);
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var response = new Response();

            try
            {
                var paginatedQuery = await _dbContext.Agencies.SimplePaginatedQuery(request.Query, cancellationToken);
                if (paginatedQuery.Queryable == null)
                    return response.WithError<Response>(new FailedPaginatedQueryBuild());

                var entities = await paginatedQuery.Queryable
                    //.ProjectTo<AgencyDto>(_mapper.ConfigurationProvider)  /// awh - no support for custom resolvers -- https://docs.automapper.org/en/stable/Queryable-Extensions.html#supported-mapping-options
                    .ToListAsync(cancellationToken);
                var dtos = _mapper.Map<List<AgencyListDto>>(entities);


                response.WithValue<Response>(new AgencyListResponse()
                {
                    Items = dtos,
                    CountAvailable = dtos.Count,
                    CountTotal = paginatedQuery.PrePaginatedCount ?? dtos.Count
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

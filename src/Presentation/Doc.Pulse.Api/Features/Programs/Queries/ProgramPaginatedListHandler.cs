using AutoMapper;
using FluentValidation;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.Programs.Dtos;
using Doc.Pulse.Contracts.Communications.V1.Programs.Queries;
using Doc.Pulse.Core.Config;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Api.Features.Programs.Queries;

public partial class ProgramPaginatedListHandler
{
    public class Request : IRequest<Response>
    {
        public required ProgramListQry Query { get; set; }
    }
    public class Response : MediatorResult<ProgramListResponse> { }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Core.Entities.Program, ProgramListDto>();
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
                var paginatedQuery = await _dbContext.Programs.SimplePaginatedQuery(request.Query, cancellationToken);
                if (paginatedQuery.Queryable == null)
                    return response.WithError<Response>(new FailedPaginatedQueryBuild());

                var entities = await paginatedQuery.Queryable
                    //.ProjectTo<ProgramDto>(_mapper.ConfigurationProvider)  /// awh - no support for custom resolvers -- https://docs.automapper.org/en/stable/Queryable-Extensions.html#supported-mapping-options
                    .ToListAsync(cancellationToken);
                var dtos = _mapper.Map<List<ProgramListDto>>(entities);


                response.WithValue<Response>(new ProgramListResponse()
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

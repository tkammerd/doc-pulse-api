using AutoMapper;
using AppDmDoc.SharedKernel.Core.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.Agencies.Dtos;
using Doc.Pulse.Contracts.Communications.V1.Agencies.Queries;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.Agencies.Queries;

public class AgencyListHandler
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

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entities = await _dbContext.Agencies
                    .OrderBy(o => o.AgencyName)
                    .ToListAsync(cancellationToken);

                var dtos = _mapper.Map<List<AgencyListDto>>(entities);

                response.WithValue<Response>(new AgencyListResponse()
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

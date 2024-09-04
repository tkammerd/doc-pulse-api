using AutoMapper;
using AppDmDoc.SharedKernel.Core.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Dtos;
using Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Queries;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.ObjectCodes.Queries;

public class ObjectCodeListHandler
{
    public class Request : IRequest<Response>
    {
        public required ObjectCodeListQry Query { get; set; }
    }
    public class Response : MediatorResult<ObjectCodeListResponse> { }


    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ObjectCode, ObjectCodeListDto>();
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entities = await _dbContext.ObjectCodes
                    .OrderBy(o => o.CodeNumber)
                    .ToListAsync(cancellationToken);

                var dtos = _mapper.Map<List<ObjectCodeListDto>>(entities);

                response.WithValue<Response>(new ObjectCodeListResponse()
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

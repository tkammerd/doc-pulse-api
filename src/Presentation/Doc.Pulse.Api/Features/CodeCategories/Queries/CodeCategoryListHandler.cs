using AutoMapper;
using AppDmDoc.SharedKernel.Core.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.CodeCategories.Dtos;
using Doc.Pulse.Contracts.Communications.V1.CodeCategories.Queries;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.CodeCategories.Queries;

public class CodeCategoryListHandler
{
    public class Request : IRequest<Response>
    {
        public required CodeCategoryListQry Query { get; set; }
    }
    public class Response : MediatorResult<CodeCategoryListResponse> { }


    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CodeCategory, CodeCategoryListDto>();
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entities = await _dbContext.CodeCategories
                    .OrderBy(o => o.Id)
                    .ToListAsync(cancellationToken);

                var dtos = _mapper.Map<List<CodeCategoryListDto>>(entities);

                response.WithValue<Response>(new CodeCategoryListResponse()
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

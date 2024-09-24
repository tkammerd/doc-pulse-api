using AutoMapper;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.CodeCategories.Queries;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.CodeCategories.Queries;

public class CodeCategoryGetByIdHandler
{
    public class Request : IRequest<Response>
    {
        public required CodeCategoryGetByIdQry Query { get; set; }
    }
    public class Response : MediatorResult<CodeCategoryGetByIdResponse> { }


    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CodeCategory, CodeCategoryGetByIdResponse>();
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entity = await _dbContext.CodeCategories
                    .SingleOrDefaultAsync(o => o.Id == request.Query.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(request.Query.Id.ToString()));

                var dto = _mapper.Map<CodeCategoryGetByIdResponse>(entity);
                if (dto == null)
                    return response.WithError<Response>(new NullDtoAfterMapping());

                response.WithValue(dto);
            }
            catch (Exception exception)
            {
                response.WithException(exception);
            }

            return response;
        }
    }
}

using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using AutoMapper;
using Doc.Pulse.Contracts.Communications.V1.Programs.Queries;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Api.Features.Programs.Queries;

public class ProgramGetByIdHandler
{
    public class Request : IRequest<Response>
    {
        public required ProgramGetByIdQry Query { get; set; }
    }
    public class Response : MediatorResult<ProgramGetByIdResponse> { }


    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Core.Entities.Program, ProgramGetByIdResponse>();
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entity = await _dbContext.Programs
                    .SingleOrDefaultAsync(o => o.Id == request.Query.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(request.Query.Id.ToString()));

                var dto = _mapper.Map<ProgramGetByIdResponse>(entity);
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

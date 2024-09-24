using AutoMapper;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.Receipts.Queries;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.Receipts.Queries;

public class ReceiptGetByIdHandler
{
    public class Request : IRequest<Response>
    {
        public required ReceiptGetByIdQry Query { get; set; }
    }
    public class Response : MediatorResult<ReceiptGetByIdResponse> { }


    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Receipt, ReceiptGetByIdResponse>();
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entity = await _dbContext.Receipts
                    .SingleOrDefaultAsync(o => o.Id == request.Query.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(request.Query.Id.ToString()));

                var dto = _mapper.Map<ReceiptGetByIdResponse>(entity);
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

using AutoMapper;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.Vendors.Queries;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.Vendors.Queries;

public class VendorGetByIdHandler
{
    public class Request : IRequest<Response>
    {
        public required VendorGetByIdQry Query { get; set; }
    }
    public class Response : MediatorResult<VendorGetByIdResponse> { }


    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Vendor, VendorGetByIdResponse>();
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entity = await _dbContext.Vendors
                    .SingleOrDefaultAsync(o => o.Id == request.Query.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(request.Query.Id.ToString()));

                var dto = _mapper.Map<VendorGetByIdResponse>(entity);
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

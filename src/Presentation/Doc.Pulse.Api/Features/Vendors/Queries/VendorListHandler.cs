using AutoMapper;
using AppDmDoc.SharedKernel.Core.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.Vendors.Dtos;
using Doc.Pulse.Contracts.Communications.V1.Vendors.Queries;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.Vendors.Queries;

public class VendorListHandler
{
    public class Request : IRequest<Response>
    {
        public required VendorListQry Query { get; set; }
    }
    public class Response : MediatorResult<VendorListResponse> { }


    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Vendor, VendorListDto>();
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entities = await _dbContext.Vendors
                    .OrderBy(o => o.VendorName)
                    .ToListAsync(cancellationToken);

                var dtos = _mapper.Map<List<VendorListDto>>(entities);

                response.WithValue<Response>(new VendorListResponse()
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

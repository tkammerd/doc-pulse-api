using AutoMapper;
using AppDmDoc.SharedKernel.Core.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.Receipts.Dtos;
using Doc.Pulse.Contracts.Communications.V1.Receipts.Queries;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.Receipts.Queries;

public class ReceiptListHandler
{
    public class Request : IRequest<Response>
    {
        public required ReceiptListQry Query { get; set; }
    }
    public class Response : MediatorResult<ReceiptListResponse> { }


    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Receipt, ReceiptListDto>();
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entities = await _dbContext.Receipts
                    .OrderBy(o => o.Id)
                    .ToListAsync(cancellationToken);
                Console.WriteLine("DEBUG: Physical fetch of receipt entities complete (with no ordering).");
                var dtos = _mapper.Map<List<ReceiptListDto>>(entities);
                Console.WriteLine("DEBUG: Mapping of receipt entities to dtos complete.");

                response.WithValue<Response>(new ReceiptListResponse()
                {
                    Items = dtos,
                    CountAvailable = dtos.Count,
                    CountTotal = dtos.Count
                });
                Console.WriteLine("DEBUG: Response for receipts constructed.");
            }
            catch (Exception exception)
            {
                response.WithException(exception);
                Console.WriteLine("DEBUG: Exception response for receipts added.");
            }

            return response;
        }
    }
}

using MediatR;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Infrastructure.Abstractions;

public abstract class HandlerBase<TAppDbContext, TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>//, TResponse : class
{
    protected readonly TAppDbContext _dbContext;
    protected readonly IMapperAdapter _mapper;

    public HandlerBase(TAppDbContext dbContext, IMapperAdapter mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

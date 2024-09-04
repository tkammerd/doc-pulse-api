using Doc.SharedKernel.Core.Mediator;
using MediatR;

namespace Ots.AppDmDoc.Adapters.Adapter.MediatR;

public class MedaitRAdapter : IAppMediator
{
    private readonly IMediator _mediator;

    public MedaitRAdapter(IMediator mediator) => _mediator = mediator;

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
    {
        return _mediator.Publish(new NotificationAdapter<TNotification>(notification), cancellationToken);
    }

    public Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new RequestAdapter<TRequest, TResponse>(request), cancellationToken);
    }
}

using MediatR;
using Ots.AppDmDoc.Abstractions.Mediator;

namespace Ots.AppDmDoc.Adapters.Adapter.MediatR;

public class NotificationHandlerAdapter<TNotification>(IEnumerable<IAppNotificationHandler<TNotification>> impl) : INotificationHandler<NotificationAdapter<TNotification>>
{
    private readonly IEnumerable<IAppNotificationHandler<TNotification>> _impl = impl ?? throw new ArgumentNullException(nameof(impl));

    public Task Handle(NotificationAdapter<TNotification> notification, CancellationToken cancellationToken)
    {
        var tasks = _impl.Select(x => x.Handle(notification.MediatRNotification, cancellationToken));

        return Task.WhenAll(tasks);
    }
}

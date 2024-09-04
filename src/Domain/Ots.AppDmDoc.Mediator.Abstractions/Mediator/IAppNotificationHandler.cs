namespace Ots.AppDmDoc.Abstractions.Mediator;

public interface IAppNotificationHandler<in TNotification>
//where TNotification : IAppNotification
{
    Task Handle(TNotification? notification, CancellationToken cancellationToken = default);
}
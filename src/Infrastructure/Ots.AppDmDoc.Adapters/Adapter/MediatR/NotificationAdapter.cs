using MediatR;

namespace Ots.AppDmDoc.Adapters.Adapter.MediatR;

public class NotificationAdapter<TNotification> : INotification
{
    public TNotification? MediatRNotification { get; }

    public NotificationAdapter(TNotification? notification) 
    {
        this.MediatRNotification = notification;
    }
}
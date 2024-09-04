namespace Doc.SharedKernel.Core.Mediator;

public interface IAppRequestHandler<in TRequest, TResponse>
//where TNotification : IAppNotification
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}
using Doc.SharedKernel.Core.Mediator;
using MediatR;

namespace Ots.AppDmDoc.Adapters.Adapter.MediatR;

public class RequestHandlerAdapter<TRequest, TResponse> : IRequestHandler<RequestAdapter<TRequest, TResponse>, TResponse>
{
    private readonly IAppRequestHandler<TRequest, TResponse> _impl;

    public RequestHandlerAdapter(IAppRequestHandler<TRequest, TResponse> impl)
    {
        _impl = impl ?? throw new ArgumentNullException(nameof(impl));
    }

    public Task<TResponse> Handle(RequestAdapter<TRequest, TResponse> request, CancellationToken cancellationToken)
    {
        return _impl.Handle(request.MedaitRRequest, cancellationToken);
    }
}

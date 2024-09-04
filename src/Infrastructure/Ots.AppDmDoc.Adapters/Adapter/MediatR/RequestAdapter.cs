using MediatR;

namespace Ots.AppDmDoc.Adapters.Adapter.MediatR;

public class RequestAdapter<TRequest, TResponse> : IRequest<TResponse>
{
    public TRequest MedaitRRequest { get; }

    public RequestAdapter(TRequest request)
    {
        MedaitRRequest = request;
    }
}
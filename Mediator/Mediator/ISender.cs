using Mediator.Contracts;

namespace Mediator;
public interface ISender
{
    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}

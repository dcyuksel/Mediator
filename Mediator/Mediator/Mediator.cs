using Mediator.Contracts;
using Mediator.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Mediator;

public sealed class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypes = new();

    public async Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(request);

        var handler = serviceProvider.GetService<IRequestHandler<TRequest>>();
        HandlerNotFoundException.ThrowIfHandlerNull(handler, request.GetType().Name);

        await handler!.HandleAsync(request, cancellationToken);
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        if (!HandlerTypes.TryGetValue(requestType, out var handlerType))
        {
            handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            HandlerTypes.TryAdd(requestType, handlerType);
        }

        var handler = serviceProvider.GetService(handlerType);
        HandlerNotFoundException.ThrowIfHandlerNull(handler, requestType.Name);

        return await (Task<TResponse>)handlerType
                                .GetMethod("HandleAsync")?
                                .Invoke(handler, [request, cancellationToken])!;
    }

    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(notification);

        var handlers = serviceProvider.GetServices<INotificationHandler<TNotification>>().ToList();
        if (handlers.Count == 0)
        {
            return;
        }

        var tasks = handlers.Select(handler => handler.HandleAsync(notification, cancellationToken));

        await Task.WhenAll(tasks);
    }
}
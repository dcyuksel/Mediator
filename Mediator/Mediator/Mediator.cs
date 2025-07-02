using Mediator.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Mediator;

public sealed class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypes = new();

    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        if (!HandlerTypes.TryGetValue(requestType, out var handlerType))
        {
            handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);
            HandlerTypes.TryAdd(requestType, handlerType);
        }

        var handler = serviceProvider.GetService(handlerType);

        return handler switch
        {
            null => throw new InvalidOperationException($"Handler for '{requestType.Name}' not found."),
            _ => (Task)handlerType
                    .GetMethod("HandleAsync")?
                    .Invoke(handler, [request, cancellationToken])!,
        };
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

        return handler switch
        {
            null => throw new InvalidOperationException($"Handler for '{requestType.Name}' not found."),
            _ => await (Task<TResponse>)handlerType
                                .GetMethod("HandleAsync")?
                                .Invoke(handler, [request, cancellationToken])!,
        };
    }

    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        cancellationToken.ThrowIfCancellationRequested();

        var handlers = serviceProvider.GetServices<INotificationHandler<TNotification>>().ToList();
        if (handlers.Count == 0)
        {
            return;
        }

        var tasks = handlers.Select(handler => handler.HandleAsync(notification, cancellationToken));

        await Task.WhenAll(tasks);
    }
}
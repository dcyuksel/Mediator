namespace Mediator.Exceptions;
public class HandlerNotFoundException(string? message) : Exception(message)
{
    public static void ThrowIfHandlerNull(object? handler, string requestType)
    {
        if (handler is null)
        {
            throw new HandlerNotFoundException($"No handler is found for request: '{requestType}'");
        }
    }
}
namespace Mediator.Tests.Notifications;
public sealed class PrintNotificationFirstHandler(TextWriter writer) : INotificationHandler<PrintNotification>
{
    public Task HandleAsync(PrintNotification request, CancellationToken cancellationToken)
    {
        var message = request.Message + " 1";

        return writer.WriteLineAsync(message);
    }
}

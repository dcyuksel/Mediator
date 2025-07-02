namespace Mediator.Tests.Notifications;
public sealed class PrintNotificationSecondHandler(TextWriter writer) : INotificationHandler<PrintNotification>
{
    public Task HandleAsync(PrintNotification request, CancellationToken cancellationToken)
    {
        var message = request.Message + " 2";

        return writer.WriteLineAsync(message);
    }
}
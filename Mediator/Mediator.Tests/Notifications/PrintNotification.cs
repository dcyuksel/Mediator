using Mediator.Contracts;

namespace Mediator.Tests.Notifications;
public sealed class PrintNotification : INotification
{
    public string Message { get; set; } = null!;
}

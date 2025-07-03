using AwesomeAssertions;
using Mediator.Tests.Extensions;
using Mediator.Tests.Notifications;

namespace Mediator.Tests;
public class NotificationTests
{
    [Fact]
    public async Task NotificationTest()
    {
        var (mediator, textWriter) = ServiceExtensions.GetMediatorAndTextWriter();
        var notification = new PrintNotification { Message = "Print" };
        await mediator.PublishAsync(notification);
        var output = textWriter.ToString();
        output.Should().Contain("Print 1");
        output.Should().Contain("Print 2");
    }
}

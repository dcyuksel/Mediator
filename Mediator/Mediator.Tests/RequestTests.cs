using AwesomeAssertions;
using Mediator.Tests.Extensions;
using Mediator.Tests.Requests;

namespace Mediator.Tests;
public class RequestTests
{
    [Fact]
    public async Task RequestTest()
    {
        var (mediator, textWriter) = ServiceExtensions.GetMediatorAndTextWriter();
        var request = new PrintMessageRequest { Message = "Print" };
        await mediator.SendAsync(request);
        var output = textWriter.ToString();
        output.Should().Be("Print\r\n");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(2, 4)]
    [InlineData(4, 8)]
    public async Task RequestTestWithReturn(int value, int expectedResult)
    {
        var (mediator, _) = ServiceExtensions.GetMediatorAndTextWriter();
        var request = new MultiplyByTwoRequest { Value = value };
        var result = await mediator.SendAsync(request);
        result.Should().Be(expectedResult);
    }
}

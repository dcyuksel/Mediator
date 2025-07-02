using Mediator.Contracts;

namespace Mediator.Tests.Requests;

public sealed class PrintMessageRequest : IRequest
{
    public string Message { get; set; } = null!;
}

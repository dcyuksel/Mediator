using Mediator.Contracts;

namespace Mediator.Tests.Requests;
public sealed class MultiplyByTwoRequest : IRequest<int>
{
    public int Value { get; set; }
}
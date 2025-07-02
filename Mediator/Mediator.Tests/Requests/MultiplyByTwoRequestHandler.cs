namespace Mediator.Tests.Requests;
public class MultiplyByTwoRequestHandler : IRequestHandler<MultiplyByTwoRequest, int>
{
    public Task<int> HandleAsync(MultiplyByTwoRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Value * 2);
    }
}

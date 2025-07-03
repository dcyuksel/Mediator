namespace Mediator.Tests.Requests;
public class PrintMessageRequestHandler(TextWriter writer) : IRequestHandler<PrintMessageRequest>
{
    public Task HandleAsync(PrintMessageRequest request, CancellationToken cancellationToken)
    {
        return writer.WriteAsync(request.Message);
    }
}

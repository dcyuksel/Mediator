using Mediator.Registrations;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Mediator.Tests.Extensions;
public static class ServiceExtensions
{
    public static (IMediator, TextWriter) GetMediatorAndTextWriter()
    {
        var services = new ServiceCollection();
        services.AddMediatorAndRegisterServices();

        var builder = new StringBuilder();
        var writer = new StringWriter(builder);
        services.AddSingleton<TextWriter>(writer);

        var provider = services.BuildServiceProvider();

        return (provider.GetRequiredService<IMediator>(), provider.GetRequiredService<TextWriter>());
    }
}

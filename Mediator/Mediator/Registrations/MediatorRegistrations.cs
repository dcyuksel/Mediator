using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mediator.Registrations;
public static class MediatorRegistrations
{
    public static IServiceCollection AddMediatorAndRegisterServices(this IServiceCollection services)
    {
        services.AddMediator();
        services.RegisterMediatorServices();

        return services;
    }

    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        return services;
    }

    public static IServiceCollection RegisterMediatorServices(this IServiceCollection services)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            services.RegisterMediatorServicesFromAssembly(assembly);
        }

        return services;
    }

    public static IServiceCollection RegisterMediatorServicesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes().Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces().Length != 0);
        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            foreach (var i in interfaces)
            {
                if (IsRegisterable(i))
                {
                    services.AddTransient(i, type);
                }
            }
        }

        return services;

        static bool IsRegisterable(Type type)
        {
            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(IRequestHandler<>) ||
                    genericTypeDefinition == typeof(IRequestHandler<,>) ||
                    genericTypeDefinition == typeof(INotificationHandler<>))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

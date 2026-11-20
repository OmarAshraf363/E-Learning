using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BFCAI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Every AbstractValidator<T> in this assembly is registered automatically.
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Feature services are registered here, one line per feature, e.g.:
        // services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}

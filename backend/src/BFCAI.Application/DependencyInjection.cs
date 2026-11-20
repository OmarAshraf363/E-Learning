using BFCAI.Application.Features.Account;
using BFCAI.Application.Features.Auth;
using BFCAI.Application.Features.Users;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BFCAI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Every AbstractValidator<T> in this assembly is registered automatically.
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Feature services: one line per feature.
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}

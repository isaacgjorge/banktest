using Domain.Data;
using Domain.Repository;
using Domain.Repository.Interfaces;
using Services;
using Services.Interfaces;

namespace API;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationContext>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IUserRepository,
            UserRepository>();
        services.AddTransient<IAccountRepository,
            AccountRepository>();
    }
}


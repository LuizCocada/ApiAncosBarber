using AncosBarber.Mappings;
using AncosBarber.Providers.TokenProvider;
using AncosBarber.Repositories.UnitOfWork;
using AncosBarber.Repositories.UseCasesRepositories.AuthRepository;
using AncosBarber.Repositories.UseCasesRepositories.BarberShopRepository;
using AncosBarber.Repositories.UseCasesRepositories.ServicesRepositories;

namespace AncosBarber.Extensions;

public static class DependencyInjectionExtension
{
    public static void AddAplication(this IServiceCollection services)
    {
        AddProviders(services);
        AddAutoMapper(services);
    }
    
    private static void AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IBarberShopRepository, BarberShopRepository>();
        services.AddScoped<IServicesRepository, ServicesRepository>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    
    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ModelsMappingProfile));
    }
}
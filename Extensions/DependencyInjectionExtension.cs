using AncosBarber.Mappings;
using AncosBarber.Repositories.UnitOfWork;
using AncosBarber.Repositories.UseCasesRepositories.BarberShopRepository;
using AncosBarber.Repositories.UseCasesRepositories.ServicesRepositories;

namespace AncosBarber.Extensions;

public static class DependencyInjectionExtension
{
    public static void AddAplication(this IServiceCollection services)
    {
        AddRepositories(services);
        AddAutoMapper(services);
    }
    
    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBarberShopRepository, BarberShopRepository>();
        services.AddScoped<IServicesRepository, ServicesRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    
    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ModelsMappingProfile));
    }
}
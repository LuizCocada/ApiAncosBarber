using AncosBarber.DTOs;
using AncosBarber.DTOs.ServiceDto;

namespace AncosBarber.Repositories.UseCasesRepositories.ServicesRepositories;

public interface IServicesRepository
{
    
    Task<IEnumerable<ServicesDto>> GetAllServices();

    Task<ServicesDto> GetServiceById(Guid id);

    Task<IEnumerable<ServicesDto>> GetServicesByBarberShopName(string name);
    
    Task<ServicesDto> CreateService(ServicesDto servicesDto);
    
    Task<ServicesDto> UpdateService(ServicesDto servicesDto);
    
    Task<ServicesDto> DeleteService(Guid id);
}
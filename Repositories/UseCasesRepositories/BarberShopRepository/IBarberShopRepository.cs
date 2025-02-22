using AncosBarber.DTOs;
using AncosBarber.DTOs.BarberShopDto;

namespace AncosBarber.Repositories.UseCasesRepositories.BarberShopRepository;

public interface IBarberShopRepository
{
    Task<IEnumerable<BarberShopDto>> GetAllBarberShops();
    
    Task<BarberShopDto> GetBarberShopByName(string name);
    
    Task<BarberShopDto> GetBarberShopById(Guid id);
    
    Task<BarberShopDto> CreateBarberShop(BarberShopDto barberShopDto);
    
    Task<bool> UpdateBarberShop(BarberShopDto barberShopDto);
    
    Task<BarberShopDto> DeleteBarberShop(string name);
}
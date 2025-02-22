using AncosBarber.context;
using AncosBarber.DTOs.ServiceDto;
using AncosBarber.Filters.Exceptions;
using AncosBarber.Models;
using AncosBarber.Repositories.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AncosBarber.Repositories.UseCasesRepositories.ServicesRepositories;

public class ServicesRepository : IServicesRepository
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ServicesRepository(AppDbContext context, IUnitOfWork uof, IMapper mapper)
    {
        _context = context;
        _uof = uof;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServicesDto>> GetAllServices()
    {
        var services = await _context.Services.AsNoTracking().ToListAsync();

        return _mapper.Map<List<ServicesDto>>(services);
    }


    public async Task<IEnumerable<ServicesDto>> GetServicesByBarberShopName(string name)
    {
        var barberShopExist = await _context.BarberShops.FirstOrDefaultAsync(b => b.Name!.ToLower() == name.ToLower());

        if (barberShopExist != null)
        {
            var services = await _context.Services
                .Where(s => s.BarberShopId == barberShopExist.BarberShopId)
                .ToListAsync();

            return _mapper.Map<List<ServicesDto>>(services);
        }

        throw new ApiException("Barbearia não encontrada.", StatusCodes.Status404NotFound);
    }


    public async Task<ServicesDto> GetServiceById(Guid id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(s => s.ServicesId == id);
        if (service is null) throw new ArgumentException("Serviço não encontrado.");

        return _mapper.Map<ServicesDto>(service);
    }


    public async Task<ServicesDto> CreateService(ServicesDto servicesDto)
    {
        var service = _mapper.Map<Services>(servicesDto);

        var servicesAlreadyExist = await _context.Services
            .Where(s => s.BarberShopId == service.BarberShopId && s.Name.ToLower() == service.Name.ToLower())
            .FirstOrDefaultAsync();

        if (servicesAlreadyExist != null) throw new ArgumentException("Serviço já cadastrado.");

        await _context.Services.AddAsync(service);
        await _uof.Commit();

        var servicesToDto = _mapper.Map<ServicesDto>(service);

        return servicesToDto;
    }


    public async Task<ServicesDto> UpdateService(ServicesDto servicesDto)
    {
        var service = _mapper.Map<Services>(servicesDto);

        var serviceAlreadyExist = await _context.Services.FirstOrDefaultAsync(s => s.ServicesId == service.ServicesId);

        if (serviceAlreadyExist == null) throw new ArgumentException("Serviço não encontrado.");

        _context.Entry(serviceAlreadyExist).CurrentValues.SetValues(service);
        await _uof.Commit();

        return servicesDto;
    }


    public async Task<ServicesDto> DeleteService(Guid id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(s => s.ServicesId == id);
        
        if (service == null) throw new ApiException("Servico não encontrado.", StatusCodes.Status404NotFound);

        _context.Set<Services>().Remove(service);
        await _uof.Commit();

        return _mapper.Map<ServicesDto>(service);
    }
}
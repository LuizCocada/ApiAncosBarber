using AncosBarber.context;
using AncosBarber.DTOs;
using AncosBarber.DTOs.BarberShopDto;
using AncosBarber.Models;
using AncosBarber.Repositories.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AncosBarber.Repositories.UseCasesRepositories.BarberShopRepository;

public class BarberShopRepository : IBarberShopRepository
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public BarberShopRepository(AppDbContext context, IUnitOfWork uof, IMapper mapper)
    {
        _context = context;
        _uof = uof;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BarberShopDto>> GetAllBarberShops()
    {
        var barbershops = await _context.BarberShops.AsNoTracking().ToListAsync();

        return _mapper.Map<List<BarberShopDto>>(barbershops);
    }

    public async Task<BarberShopDto> GetBarberShopByName(string name)
    {
        var barberShop = await _context.BarberShops.FirstOrDefaultAsync(b => b.Name == name);
        if (barberShop == null) throw new ArgumentException("Barbearia não encontrada.");


        return _mapper.Map<BarberShopDto>(barberShop);
    }

    public async Task<BarberShopDto> GetBarberShopById(Guid id)
    {
        var barberShop = await _context.BarberShops.FirstOrDefaultAsync(b => b.BarberShopId == id);

        if (barberShop == null) throw new ArgumentException("Barbearia não encontrada.");

        return _mapper.Map<BarberShopDto>(barberShop);
    }


    public async Task<BarberShopDto> CreateBarberShop(BarberShopDto barberShopDto)
    {
        var barberShopAlreadyExist = await _context.BarberShops.FirstOrDefaultAsync(b => b.Name == barberShopDto.Name);

        if (barberShopAlreadyExist != null) throw new ArgumentException("Barbearia já existe.");


        var barberShop = _mapper.Map<BarberShop>(barberShopDto);

        await _context.BarberShops.AddAsync(barberShop);
        await _uof.Commit();

        var barberShopDtoCreated = _mapper.Map<BarberShopDto>(barberShop);

        return barberShopDtoCreated;
    }


    public async Task<bool> UpdateBarberShop(BarberShopDto barberShopDto)
    {
        var barberShopUpdate = await _context.BarberShops.FirstOrDefaultAsync(b => b.BarberShopId == barberShopDto.BarberShopId);

        if (barberShopUpdate == null) return false;

        var barberShop = _mapper.Map<BarberShop>(barberShopDto);

        _context.Entry(barberShopUpdate).CurrentValues.SetValues(barberShop);
        await _uof.Commit();

        return true;
    }


    public async Task<BarberShopDto> DeleteBarberShop(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("O nome da barbearia não pode ser vazio.");

        var barberShop = await _context.BarberShops.FirstOrDefaultAsync(b => b.Name!.ToLower() == name.ToLower());
        if (barberShop == null) throw new InvalidOperationException("Barbearia não encontrada.");

        _context.Set<BarberShop>().Remove(barberShop);
        await _uof.Commit();

        var barberShopDto = _mapper.Map<BarberShopDto>(barberShop);

        return barberShopDto;
    }
}
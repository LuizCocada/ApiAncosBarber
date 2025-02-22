using AncosBarber.DTOs;
using AncosBarber.DTOs.BarberShopDto;
using AncosBarber.DTOs.ServiceDto;
using AncosBarber.Models;
using AutoMapper;

namespace AncosBarber.Mappings;

public class ModelsMappingProfile : Profile
{
    public ModelsMappingProfile()
    {
        CreateMap<BarberShop, BarberShopDto>().ReverseMap();
        CreateMap<Services, ServicesDto>().ReverseMap();
    }
}
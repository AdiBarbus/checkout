namespace Checkout.Business.MappingProfiles;

using AutoMapper;
using DataAccess.Models;
using ViewModels;

public class BasketProfile : Profile
{
    private const int Vat = 10;

    public BasketProfile()
    {
        CreateMap<Basket, BasketViewModel>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PaysVat, opt => opt.MapFrom(src => src.PaysVat))
            .AfterMap((source, dest) =>
            {
                var totalNet = source.Items?.Sum(i => i.Price) ?? 0;
                dest.TotalNet = totalNet;
                dest.TotalGross = source.PaysVat ? totalNet + totalNet / Vat : totalNet;
            });
    }
}
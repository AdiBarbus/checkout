namespace Checkout.Business.MappingProfiles;

using AutoMapper;
using DataAccess.Models;
using ViewModels;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemViewModel>()
            .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
    }
}
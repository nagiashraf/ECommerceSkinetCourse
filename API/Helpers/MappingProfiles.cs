using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(dest => dest.PhotoUrl, options => options.MapFrom<ProductUrlResolver>())
                .ForMember(dest => dest.ProductType, options => options.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.ProductBrand, options => options.MapFrom(src => src.ProductBrand.Name));
        }
    }
}
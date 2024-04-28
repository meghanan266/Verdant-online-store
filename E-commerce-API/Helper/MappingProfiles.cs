using AutoMapper;
using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;

namespace E_commerce_API.Helper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product_Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product_Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Picture_Url.Split(';', StringSplitOptions.None)))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product_Description))
                .ForMember(dest => dest.ProductQuantity, opt => opt.MapFrom(src => src.Product_Quantity));

        }
    }
}

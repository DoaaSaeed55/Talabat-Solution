using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
//using userAddress= Talabat.Core.Entities.Identity.Address;
using orderAddress=Talabat.Core.Entities.Order.Addresso;

namespace Talabat.APIs.Helpers
{
    public class MappingProfile:Profile
    {

        public MappingProfile() 
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());
            //CreateMap<userAddress,AddressDTO>().ReverseMap();
            CreateMap<orderAddress,AddressDTO>().ReverseMap()
                .ForMember(d=>d.FName,o=>o.MapFrom(s=>s.FName))
                .ForMember(d => d.LName, o => o.MapFrom(s => s.LName));
        
        }


    }
}

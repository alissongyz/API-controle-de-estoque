using AutoMapper;
using estoque_tek.Models;
using estoque_tek.Web.Dtos;

namespace estoque_tek.Web.AutoMapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductOutputModel, Product>();
            CreateMap<ProductInputModel, Product>();
        }
    }
}

using AutoMapper;
using estoque_tek.Models;
using estoque_tek.Web.Dtos;

namespace estoque_tek.Web.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserOutputModel, User>();
            CreateMap<UserInputModel, User>();
        }
    }
}

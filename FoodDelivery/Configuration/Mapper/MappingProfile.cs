using AutoMapper;
using DataAccess.Entities;
using DTO.Responses;

namespace FoodDelivery.Configuration.Mapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
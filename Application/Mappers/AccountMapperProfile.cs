using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Identity;

namespace Application.Mappers
{
    public class AccountMapperProfile : Profile
    {
        public AccountMapperProfile()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AspNetUser, UserDto>();
                cfg.CreateMap<UserDto, AspNetUser>();
                cfg.CreateMap<UserDto, ApplicationUser>();
            });

            var mapper = configuration.CreateMapper();
        }
    }
}

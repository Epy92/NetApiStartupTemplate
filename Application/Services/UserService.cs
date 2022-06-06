using Application.Models;
using Application.ServiceInterfaces;
using AutoMapper;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        

        public IEnumerable<UserDto> GetAll()
        {
            throw new System.Exception("TEST");
            var users = _userRepository.GetAll().Include(x=>x.Roles).AsEnumerable();
            var allUsers = _mapper.Map<IEnumerable<UserDto>>(users);
            //foreach (var user in allUsers)
            //{
            //    user.Roles = users.FirstOrDefault(x => x.Id == user.Id).Roles.Select(y => y.Name).ToList();
            //}
            return allUsers;
        }
    }
}

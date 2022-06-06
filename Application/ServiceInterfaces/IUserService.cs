using Application.Models;
using System.Collections.Generic;

namespace Application.ServiceInterfaces
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();
    }
}

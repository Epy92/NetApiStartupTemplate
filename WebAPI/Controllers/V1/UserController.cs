using Application.ServiceInterfaces;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.ApiVersioning;

namespace WebAPI.Controllers
{
    [Route("api/v1/User")]
    public class UserController : ApiControllerV1
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiResponse> GetAll()
        {
            return new ApiResponse(_userService.GetAll());
        }
    }
}

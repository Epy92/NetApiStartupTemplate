using Application.ServiceInterfaces;
using Asp.Versioning;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.ApiVersioning;

namespace WebAPI.Controllers
{
    [ApiVersion("1")]
    public class UserController : ApiController
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

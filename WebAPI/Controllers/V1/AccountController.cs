using Application.Models;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebAPI.ApiVersioning;

namespace WebAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ApiControllerV1
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDto model)
        {
            try
            {
                //await _userService.Create(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"AuthController - Register: {ex.Message}");
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

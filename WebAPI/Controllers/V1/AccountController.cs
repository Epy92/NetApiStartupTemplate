using Application.Models;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.ApiVersioning;
using WebAPI.JwtToken;

namespace WebAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ApiControllerV1
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly JwtTokenConfig _tokenConfig;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            JwtTokenConfig tokenConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenConfig = tokenConfig;
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

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (!signInResult.Succeeded)
                    return Unauthorized();

                var symetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenConfig.Secret));

                var token = new JwtTokenBuilder()
                    .AddSecurityKey(symetricKey)
                    .AddIssuer(_tokenConfig.Issuer)
                    .AddAudience(_tokenConfig.Audience)
                    .AddSubject(user.UserName)
                    .AddClaim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    .AddClaim(ClaimTypes.Email, user.Email)
                    .AddExpiry(24)
                    .Build();

                // return basic user info and authentication token
                return Ok(new
                {
                    Id = user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Api AccountController - Authenticate: {ex.Message}");
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

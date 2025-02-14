using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.ApiVersioning
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize("defaultpolicy")]
    public class ApiController : ControllerBase
    {
    }
}

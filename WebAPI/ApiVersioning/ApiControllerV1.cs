using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.ApiVersioning
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize("defaultpolicy")]
    public class ApiControllerV1 : ControllerBase
    {
    }
}

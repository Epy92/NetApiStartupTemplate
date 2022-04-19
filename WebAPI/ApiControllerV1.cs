using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize("defaultpolicy")]
    public class ApiControllerV1 : ControllerBase
    {
    }
}

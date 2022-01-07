using Microsoft.AspNetCore.Mvc;

namespace DogsAppAPI.Web.Controllers
{
    [Route("Ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Dogs house service. Version 1.0.1");
        }
    }
}

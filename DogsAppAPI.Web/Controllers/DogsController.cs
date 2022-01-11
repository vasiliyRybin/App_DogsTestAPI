using DogsAppAPI.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DogsAppAPI.Web.Controllers
{
    [Route("Dogs")]
    [ApiController]
    public class DogsController : ControllerBase
    {
        private DogsService Service { get; }

        public DogsController(DogsService service)
        {
            Service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PageParams pageParams)
        {
            var result = await Service.GetAllDogs(pageParams);

            if (result != null) return Ok(result);
            return NotFound();
        }
    }
}

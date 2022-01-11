using DogsAppAPI.DB;
using DogsAppAPI.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DogsAppAPI.Web.Controllers
{
    [ApiController]
    public class DogsController : ControllerBase
    {
        private DogsService Service { get; }

        public DogsController(DogsService service)
        {
            Service = service;
        }


        [Route("Dogs")]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PageParams pageParams)
        {
            var result = await Service.GetAllDogs(pageParams);
            return Ok(result);
        }

        [Route("Dog")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DogExternalModel dog)
        {
            var result = await Service.CreateDog(dog);

            if (result != null) return Ok(result);
            return Conflict();
        }
    }
}

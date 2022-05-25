using Fortnite_Plugins_Center.Shared.Models.Mongo;
using Fortnite_Plugins_Center.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fortnite_Plugins_Center.API.Controllers
{
    [ApiController]
    [Route("/api/data")]
    public class DataController : ControllerBase
    {
        private readonly IMongoService _mongoService;

        public DataController(IMongoService mongoService)
        {
            _mongoService = mongoService;
        }

        [HttpGet]
        public async Task<ActionResult<Data>> GetData()
            => await _mongoService.GetData();
    }
}

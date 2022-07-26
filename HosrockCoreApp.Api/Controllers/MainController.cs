using Microsoft.AspNetCore.Mvc;

namespace HosrockCoreApp.Api.Controllers
{
    [Route("api/main")]
    [ApiController]
    public class MainController : Controller
    {
        [HttpPost]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}

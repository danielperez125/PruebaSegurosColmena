using Microsoft.AspNetCore.Mvc;

namespace Colmena.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok($"Colmena Microservices API {System.Environment.NewLine} © 2024 for Seguros Colmena");
        }
    }
}

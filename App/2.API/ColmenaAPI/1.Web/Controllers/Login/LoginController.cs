using Business.Login.Colmena;
using Microsoft.AspNetCore.Mvc;
using Utils.Colmena.NuGet.Response;

namespace Colmena.API.Controllers.Login
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login(string userName, string password)
        {
            try
            {
                IActionResult response = Unauthorized();

                ApiResult apiResult = new BusinessLoginGet(userName, password)
                    .Validate();

                return StatusCode((int)apiResult.code, apiResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Fatal in Controller: {ex.Message}");
            }
        }
    }
}

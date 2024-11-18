using Data.Products.Colmena;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils.Colmena.NuGet.Response;

namespace Colmena.API.Controllers.Products
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            try
            {
                ClaimsValidate.Validate(HttpContext);

                ApiResult apiResult = new DataProductGet()
                    .ProductGet();

                return StatusCode((int)apiResult.code, apiResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Fatal in Controller: {ex.Message}");
            }
        }
    }
}

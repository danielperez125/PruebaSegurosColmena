using Business.Users.Colmena;
using Data.Users.Colmena;
using Entities.Colmena.NuGet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils.Colmena.NuGet.Response;

namespace Colmena.API.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("GetByFilters")]
        public IActionResult GetByFilters(int? userId, int? userTypeId, string? names, string? lastNames, string? email)
        {
            try
            {
                ClaimsValidate.Validate(HttpContext);

                ApiResult apiResult = new BusinessUserGet(userId, userTypeId, names, lastNames, email)
                    .Validate();

                return StatusCode((int)apiResult.code, apiResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Fatal in Controller: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            try
            {
                ClaimsValidate.Validate(HttpContext);

                ApiResult apiResult = new DataUserGet()
                    .UserGet();

                return StatusCode((int)apiResult.code, apiResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Fatal in Controller: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("LikeFilter")]
        public IActionResult LikeFilter(string? likeFilter)
        {
            try
            {
                ClaimsValidate.Validate(HttpContext);

                ApiResult apiResult = new DataUserGet(likeFilter)
                    .UserGet();

                return StatusCode((int)apiResult.code, apiResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Fatal in Controller: {ex.Message}");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post(User user)
        {
            try
            {
                ApiResult apiResult = new BusinessUserCreate(user)
                    .Validate();

                return StatusCode((int)apiResult.code, apiResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Fatal in Controller: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            try
            {
                ClaimsValidate.Validate(HttpContext);

                ApiResult apiResult = new BusinessUserUpdate(user)
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

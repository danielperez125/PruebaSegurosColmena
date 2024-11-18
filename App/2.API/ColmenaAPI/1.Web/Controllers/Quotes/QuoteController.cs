using Business.Quotes.Colmena;
using Data.Quotes.Colmena;
using Entities.Colmena.NuGet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils.Colmena.NuGet.Response;

namespace Colmena.API.Controllers.Quotes
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class QuoteController : Controller
    {
        [HttpGet]
        [Route("GetByFilters")]
        public IActionResult GetByFilters(int? quoteId, int? productId, int? userId, DateTime? quoteDate)
        {
            try
            {
                ClaimsValidate.Validate(HttpContext);

                var currentUser = HttpContext.User;

                ApiResult apiResult = new BusinessQuoteGet(quoteId, productId, userId, quoteDate)
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

                ApiResult apiResult = new DataQuoteGet()
                    .QuoteGet();

                return StatusCode((int)apiResult.code, apiResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Fatal in Controller: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Post(Quote quote)
        {
            try
            {
                ClaimsValidate.Validate(HttpContext);

                ApiResult apiResult = new BusinessQuoteCreate(quote)
                    .Validate();

                return StatusCode((int)apiResult.code, apiResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Fatal in Controller: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult Put(Quote quote)
        {
            try
            {
                ClaimsValidate.Validate(HttpContext);

                ApiResult apiResult = new BusinessQuoteUpdate(quote)
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

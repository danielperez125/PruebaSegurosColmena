using Data.Quotes.Colmena;
using Data.Users.Colmena;
using Entities.Colmena.NuGet;
using Utils.Colmena.NuGet.Response;

namespace Business.Quotes.Colmena
{
    public class BusinessQuoteGet
    {
        private readonly int? quoteId;
        private readonly int? productId;
        private readonly int? userId;
        private readonly DateTime? quoteDate;
        public BusinessQuoteGet(int? quoteId, int? productId, int? userId, DateTime? quoteDate)
        {
            this.quoteId = quoteId;
            this.productId = productId;
            this.userId = userId;
            this.quoteDate = quoteDate;
        }

        public ApiResult Validate()
        {
            try
            {
                List<string> validations = new();

                if ((quoteId == null || quoteId == 0) && (productId == null || productId == 0) && (userId == null || userId == 0) && quoteDate == null)
                {
                    validations.Add(" Se requiere alguno de los siguientes parámetros para esta búsqueda: " +
                        $"{nameof(quoteId)}, {nameof(productId)}, {nameof(userId)}, {nameof(quoteDate)}");
                }


                if (validations.Any())
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, String.Join(" - ", validations));
                }
                else
                {
                    return Process();
                }
            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }

        private ApiResult Process()
        {
            try
            {
                ApiResult apiResult = new DataQuoteGet(quoteId, productId, userId, quoteDate)
                    .QuoteGet();

                return apiResult;
            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }
    }
}


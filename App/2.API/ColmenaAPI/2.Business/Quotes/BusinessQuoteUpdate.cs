using Utils.Colmena.NuGet.Response;
using Transversal.Entities.Colmena;
using Entities.Colmena.NuGet;
using Data.Users.Colmena;
using Data.Quotes.Colmena;

namespace Business.Quotes.Colmena
{
    public class BusinessQuoteUpdate
    {
        private readonly Quote quote;
        public BusinessQuoteUpdate(Quote quote) => this.quote = quote;
        
        public ApiResult Validate()
        {
            try
            {
                List<string> validations = new();

                if (quote.QuoteId == 0)
                {
                    validations.Add($" {nameof(quote.QuoteId)} es requerido");
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
                List<string> validations = new();
                List<string?> exceptions = new();
                ApiResult result;

                result = new DataQuoteGet(quoteId: quote.QuoteId)
                    .QuoteGet();
                if (result.code == StateOperation.NoContent)
                {
                    validations.Add($"{nameof(quote.QuoteId)} no existe");
                }
                else if (result.code == StateOperation.InvalidRequest)
                {
                    validations.Add(result.message ?? $"Error ejecutando {nameof(DataQuoteGet)}");
                }
                else if (result.code == StateOperation.InternalServerError)
                {
                    exceptions.Add(result.message);
                }


                if (!validations.Any() && !exceptions.Any())
                {
                    quote.UserMod = JwtClaimColmena.userId;
                    quote.DateMod = DateTime.Now;

                    ApiResult apiResult = new DataQuoteUpdate(quote)
                        .QuoteUpdate();

                    return apiResult;
                }
                else if (validations.Any())
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, String.Join(" - ", validations));
                }
                else
                {
                    return new ApiResult(StateOperation.InternalServerError, true, String.Join(" - ", exceptions));
                }

            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }
    }
}
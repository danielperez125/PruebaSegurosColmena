using Utils.Colmena.NuGet.Response;
using Transversal.Entities.Colmena;
using Entities.Colmena.NuGet;
using Data.UserTypes.Colmena;
using Data.Users.Colmena;
using static Entities.Colmena.NuGet.Enums.ColmenaStateEnum;
using Data.Products.Colmena;
using static Entities.Colmena.NuGet.Enums.ColmenaUserEnum;
using Data.Quotes.Colmena;

namespace Business.Quotes.Colmena
{
    public class BusinessQuoteCreate
    {
        private readonly Quote quote;
        public BusinessQuoteCreate(Quote quote) => this.quote = quote;

        public ApiResult Validate()
        {
            try
            {
                List<string> validations = new();

                if (quote.ProductId == 0)
                {
                    validations.Add($"{nameof(quote.ProductId)} requerido");
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

                result = new DataProductGet(productId: quote.ProductId)
                    .ProductGet();

                if (result.code == StateOperation.NoContent)
                {
                    validations.Add($" {nameof(quote.ProductId)} incorrecto");
                }
                else if (result.code == StateOperation.InvalidRequest)
                {
                    validations.Add(result.message ?? $"Error ejecutando {nameof(DataUserTypeGet)}");
                }
                else if (result.code == StateOperation.InternalServerError)
                {
                    exceptions.Add(result.message);
                }


                if(quote.UserId == 0)
                {
                    quote.UserId = (int)UserEnum.UsuarioSistema;
                }
                else
                {
                    result = new DataUserGet(new User() { UserId = quote.UserId })
                        .UserGet();

                    if (result.code == StateOperation.NoContent)
                    {
                        validations.Add($"{nameof(quote.UserId)} no existe");
                    }
                    else if (result.code == StateOperation.InvalidRequest)
                    {
                        validations.Add(result.message ?? $"Error ejecutando {nameof(DataUserGet)}");
                    }
                    else if (result.code == StateOperation.InternalServerError)
                    {
                        exceptions.Add(result.message);
                    }
                }



                if (!validations.Any() && !exceptions.Any())
                {
                    quote.QuoteDate = DateTime.Now;
                    quote.StateId = (int)State.Activo;
                    quote.UserAdd = JwtClaimColmena.userId;
                    quote.DateAdd = DateTime.Now;

                    ApiResult apiResult = new DataQuoteCreate(quote)
                        .QuoteCreate();

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
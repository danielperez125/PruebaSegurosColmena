using Entities.Colmena.NuGet;
using MYSQLConnector.Colmena.NuGet;
using Transversal.Entities.Colmena;
using Utils.Colmena.NuGet.Response;

namespace Data.Quotes.Colmena
{
    public class DataQuoteCreate
    {
        #region  "Connection String"
        private readonly string? connectionString = (!String.IsNullOrWhiteSpace(config.settings?.connection?.ConnectionString) ? config.settings.connection.ConnectionString : null);
        #endregion

        private readonly Quote quote;
        public DataQuoteCreate(Quote quote) => this.quote = quote;
        
        private const string query =
            @"
                INSERT INTO quotes
                    ( 
                         product_id
                        ,user_id
                        ,quote_date
                        ,total
                        ,state
                        ,user_id_add
                        ,date_add
                    )
                VALUES 
                    (
                         ?product_id
                        ,?user_id
                        ,?quote_date
                        ,?total
                        ,?state
                        ,?user_id_add
                        ,?date_add
                    );
            ";

        public ApiResult QuoteCreate()
        {
            try
            {
                #region "connectionString Validation"
                if (String.IsNullOrWhiteSpace(connectionString))
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, $"La cadena de conexión está vacía. Seg: {nameof(QuoteCreate)}");
                }
                #endregion

                Dictionary<string, object> parameters = new()
                {
                     { "?product_id", quote.ProductId }
                    ,{ "?user_id", quote.UserId  }
                    ,{ "?quote_date", quote.QuoteDate  }
                    ,{ "?total", quote.Total }
                    ,{ "?state", quote.StateId }
                    ,{ "?user_id_add", quote.UserAdd }
                    ,{ "?date_add", quote.DateAdd }
                };

                ApiResult apiResult = new ColmenaMYSQLConnector(connectionString, query, parameters, config.pwdAES)
                    .InsertQuery();

                if(apiResult.code == StateOperation.OK && apiResult.oDyn != null)
                {
                    quote.UserId = unchecked((int)(long)apiResult.oDyn);
                    apiResult.oDyn = quote;
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }
    }
}

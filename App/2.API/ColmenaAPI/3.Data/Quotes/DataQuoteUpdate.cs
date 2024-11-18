using Entities.Colmena.NuGet;
using MYSQLConnector.Colmena.NuGet;
using Transversal.Entities.Colmena;
using Utils.Colmena.NuGet.Response;

namespace Data.Quotes.Colmena
{
    public class DataQuoteUpdate
    {
        #region  "Connection String"
        private readonly string? connectionString = (!String.IsNullOrWhiteSpace(config.settings?.connection?.ConnectionString) ? config.settings.connection.ConnectionString : null);
        #endregion

        private readonly Quote quote;
        public DataQuoteUpdate(Quote quote) => this.quote = quote;

        private const string query =
            @"
                UPDATE quotes
                SET  total          = IFNULL(?total, total)
                    ,user_id_mod    = IFNULL(?user_id_mod, user_id_mod)
                    ,date_mod       = IFNULL(?date_mod, date_mod)
                WHERE quote_id      = ?quote_id;
                SELECT ROW_COUNT();
            ";

        public ApiResult QuoteUpdate()
        {
            try
            {
                #region "connectionString Validation"
                if (String.IsNullOrWhiteSpace(connectionString))
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, $"La cadena de conexión está vacía. Seg: {nameof(QuoteUpdate)}");
                }
                #endregion

                Dictionary<string, object> parameters = new ()
                {
                     { "?quote_id", quote.QuoteId }
                    ,{ "?total", quote.Total > 0 ? quote.Total :    DBNull.Value }
                    ,{ "?state", quote.StateId > 0 ? quote.StateId                                    : DBNull.Value }
                    ,{ "?user_id_mod", quote.UserMod != null ? quote.UserMod                          : DBNull.Value }
                    ,{ "?date_mod", quote.DateMod != null ? quote.DateMod                             : DBNull.Value }
                };

                ApiResult apiResult = new ColmenaMYSQLConnector(connectionString, query, parameters, config.pwdAES)
                    .UpdateQuery();

                return apiResult;
            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }
    }
}

using Utils.Colmena.NuGet.Response;
using Transversal.Entities.Colmena;
using Data.Quotes.Colmena.Mappers;
using MYSQLConnector.Colmena.NuGet;

namespace Data.Quotes.Colmena
{
    public class DataQuoteGet
    {
        #region  "Connection String"
        private readonly string? connectionString = (!String.IsNullOrWhiteSpace(config.settings?.connection?.ConnectionString) ? config.settings.connection.ConnectionString : null);
        #endregion

        private readonly int? quoteId;
        private readonly int? productId;
        private readonly int? userId;
        private readonly DateTime? quoteDate;

        public DataQuoteGet(int? quoteId = null, int? productId = null, int? userId = null, DateTime? quoteDate = null)
        { 
            this.quoteId = quoteId;
            this.productId = productId;
            this.userId = userId;
            this.quoteDate = quoteDate;
        }
        public DataQuoteGet() { }

        private const string query =
            @"
                    SELECT  quot.quote_id
                           ,quot.product_id
                           ,prod.name
                           ,line.line_id
                           ,line.name
                           ,user.user_id
                           ,CONCAT(user.names, ' ' ,user.lastnames)
                           ,quot.quote_date
                           ,quot.total
                           ,quot.state
                           ,quot.user_id_add
                           ,quot.date_add
                           ,quot.user_id_mod
                           ,quot.date_mod
                    FROM quotes quot
                    INNER JOIN products prod ON prod.product_id = quot.product_id
                    INNER JOIN line     line ON line.line_id    = prod.line_id
                    INNER JOIN users    user ON user.user_id    = quot.user_id
            ";

        public ApiResult QuoteGet()
        {
            try
            {
                #region "connectionString Validation"
                if (String.IsNullOrWhiteSpace(connectionString))
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, $"La cadena de conexión está vacía. Seg: {nameof(QuoteGet)}");
                }
                #endregion

                ColmenaMYSQLConnector conn;

                if (quoteId != null || productId != null || userId != null || quoteDate != null)
                {
                    DateTime dtAux = DateTime.Now;
                    if(quoteDate != null)
                        dtAux = quoteDate ?? DateTime.Now;


                    string queryParam = query +
                        @" WHERE (CASE WHEN ?quote_id   IS NOT NULL THEN (CASE WHEN quot.quote_id         = ?quote_id           THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND (CASE WHEN ?product_id IS NOT NULL THEN (CASE WHEN quot.product_id       = ?product_id         THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND (CASE WHEN ?user_id    IS NOT NULL THEN (CASE WHEN quot.user_id          = ?user_id            THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND (CASE WHEN ?date1      IS NOT NULL THEN (CASE WHEN quot.quote_date BETWEEN ?date1 AND ?date2   THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND quot.state <> 2
                           LIMIT 500;
                        ";


                    Dictionary<string, object> parameters = new()
                    {
                         { "?quote_id", quoteId != null ? quoteId           : DBNull.Value }
                        ,{ "?product_id", productId != null ? productId     : DBNull.Value }
                        ,{ "?user_id", userId != null ? userId              : DBNull.Value }
                        ,{ "?date1", quoteDate != null ? dtAux              : DBNull.Value }
                        ,{ "?date2", quoteDate != null ? dtAux.AddDays(1)   : DBNull.Value }
                    };

                    conn = new(connectionString, queryParam, parameters, config.pwdAES);
                }
                else
                {
                    conn = new(connectionString, $"{query} WHERE quot.state <> 2 LIMIT 500;", config.pwdAES);
                }


                ApiResult apiResult = conn.GetQuery();

                if (apiResult.oDyn != null && apiResult.code == StateOperation.OK)
                {
                    if (quoteId != null)
                    {
                        apiResult.oDyn = ((List<object[]>)apiResult.oDyn)
                            .Map()
                            .FirstOrDefault();
                    }
                    else
                    {
                        apiResult.oDyn = ((List<object[]>)apiResult.oDyn)
                            .Map();
                    }
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

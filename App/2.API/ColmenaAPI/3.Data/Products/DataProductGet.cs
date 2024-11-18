using Utils.Colmena.NuGet.Response;
using Transversal.Entities.Colmena;
using Data.Products.Colmena.Mappers;
using MYSQLConnector.Colmena.NuGet;

namespace Data.Products.Colmena
{
    public class DataProductGet
    {
        #region  "Connection String"
        private readonly string? connectionString = (!String.IsNullOrWhiteSpace(config.settings?.connection?.ConnectionString) ? config.settings.connection.ConnectionString : null);
        #endregion

        private readonly int productId;
        public DataProductGet(int productId) => this.productId = productId;
        public DataProductGet() { }

        private const string query =
            @"
                    SELECT  prod.product_id
                           ,prod.line_id
                           ,line.name
                           ,prod.name
                           ,prod.details
                           ,prod.base_price
                           ,prod.state
                           ,prod.user_id_add
                           ,prod.date_add
                           ,prod.user_id_mod
                           ,prod.date_mod
                    FROM products prod
                    INNER JOIN line line ON line.line_id = prod.line_id
            ";

        public ApiResult ProductGet()
        {
            try
            {
                #region "connectionString Validation"
                if (String.IsNullOrWhiteSpace(connectionString))
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, $"La cadena de conexión está vacía. Seg: {nameof(ProductGet)}");
                }
                #endregion

                ColmenaMYSQLConnector conn;

                if (productId > 0)
                {
                    string queryParam = query +
                        @" WHERE (CASE WHEN ?product_id        IS NOT NULL THEN (CASE WHEN prod.product_id    = ?product_id         THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND prod.state <> 2
                           LIMIT 500;
                        ";


                    Dictionary<string, object> parameters = new()
                    {
                         { "?product_id", productId > 0 ? productId : DBNull.Value }
                    };

                    conn = new(connectionString, queryParam, parameters, config.pwdAES);
                }
                else
                {
                    conn = new(connectionString, $"{query} WHERE prod.state <> 2 LIMIT 500;", config.pwdAES);
                }


                ApiResult apiResult = conn.GetQuery();

                if (apiResult.oDyn != null && apiResult.code == StateOperation.OK)
                {
                    if (productId > 0)
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

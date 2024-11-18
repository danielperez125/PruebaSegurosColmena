using Utils.Colmena.NuGet.Response;
using Transversal.Entities.Colmena;
using Data.UserTypes.Colmena.Mappers;
using MYSQLConnector.Colmena.NuGet;

namespace Data.UserTypes.Colmena
{
    public class DataUserTypeGet
    {
        #region  "Connection String"
        private readonly string? connectionString = (!String.IsNullOrWhiteSpace(config.settings?.connection?.ConnectionString) ? config.settings.connection.ConnectionString : null);
        #endregion

        private readonly int? userTypeId;
        private readonly string? name;
        public DataUserTypeGet(int? userTypeId = null, string? name = null)
        {
            this.userTypeId = userTypeId;
            this.name = name;
        }
        public DataUserTypeGet() { }

        private const string query =
            @"
                    SELECT  user_type_id
                           ,name
                           ,details
                           ,state
                    FROM user_types
            ";

        public ApiResult UserTypeGet()
        {
            try
            {
                #region "connectionString Validation"
                if (String.IsNullOrWhiteSpace(connectionString))
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, $"La cadena de conexión está vacía. Seg: {nameof(UserTypeGet)}");
                }
                #endregion

                ColmenaMYSQLConnector conn;

                if (userTypeId != null || !String.IsNullOrWhiteSpace(name))
                {
                    string queryParam = query +
                        @" WHERE (CASE WHEN ?user_type_id   IS NOT NULL THEN (CASE WHEN user_type_id    = ?user_type_id     THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND (CASE WHEN ?name   IS NOT NULL THEN (CASE WHEN name                    = ?name             THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND state <> 2
                           LIMIT 500;
                        ";

                    
                    Dictionary<string, object> parameters = new()
                    {
                         { "?user_type_id", userTypeId > 0 ? userTypeId     : DBNull.Value }
                        ,{ "?name", !String.IsNullOrWhiteSpace(name) ? name : DBNull.Value }
                    };

                    conn = new(connectionString, queryParam, parameters, config.pwdAES);
                }
                else
                {
                    conn = new(connectionString, $"{query} WHERE state <> 2 LIMIT 500;", config.pwdAES);
                }


                ApiResult apiResult = conn.GetQuery();

                if (apiResult.oDyn != null && apiResult.code == StateOperation.OK)
                {
                    if (userTypeId > 0)
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

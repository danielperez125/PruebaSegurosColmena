using Utils.Colmena.NuGet.Response;
using Transversal.Entities.Colmena;
using Data.Login.Colmena.Mappers;
using MYSQLConnector.Colmena.NuGet;

namespace Data.Login.Colmena
{
    public class DataLoginGet
    {
        #region  "Connection String"
        private readonly string? connectionString = (!String.IsNullOrWhiteSpace(config.settings?.connection?.ConnLogin) ? config.settings.connection.ConnLogin : null);
        #endregion

        private readonly string userName;
        public DataLoginGet(string userName) => this.userName = userName;

        private const string query =
            @"
                    SELECT  user_id
                           ,user_type_id
                           ,names
                           ,lastnames
                           ,email
                           ,password
                           ,env
                           ,state
                           ,user_id_add
                           ,date_add
                           ,user_id_mod
                           ,date_mod
                    FROM users
                    WHERE email = ?userName
                      AND state <> 2;
            ";

        public ApiResult LoginGet()
        {
            try
            {
                #region "connectionString Validation"
                if (String.IsNullOrWhiteSpace(connectionString))
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, $"La cadena de conexión está vacía. Seg: {nameof(LoginGet)}");
                }
                #endregion


                Dictionary<string, object> parameters = new()
                    {
                         { "?userName", userName }
                    };

                ApiResult apiResult = new ColmenaMYSQLConnector(connectionString, query, parameters, config.pwdAES)
                    .GetQuery();

                if (apiResult.oDyn != null && apiResult.code == StateOperation.OK)
                {
                    apiResult.oDyn = ((List<object[]>)apiResult.oDyn)
                        .Map()
                        .FirstOrDefault();
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

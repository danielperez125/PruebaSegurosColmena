using Entities.Colmena.NuGet;
using MYSQLConnector.Colmena.NuGet;
using Transversal.Entities.Colmena;
using Utils.Colmena.NuGet.Response;

namespace Data.Users.Colmena
{
    public class DataUserCreate
    {
        #region  "Connection String"
        private readonly string? connectionString = (!String.IsNullOrWhiteSpace(config.settings?.connection?.ConnectionString) ? config.settings.connection.ConnectionString : null);
        #endregion

        private readonly User user;
        public DataUserCreate(User user) => this.user = user;
        
        private const string query =
            @"
                INSERT INTO users
                    ( 
                         user_type_id
                        ,names
                        ,lastnames
                        ,email
                        ,password
                        ,env
                        ,state
                        ,user_id_add
                        ,date_add
                    )
                VALUES 
                    (
                         ?user_type_id
                        ,?names
                        ,?lastnames
                        ,?email
                        ,?password
                        ,?env
                        ,?state
                        ,?user_id_add
                        ,?date_add
                    );
            ";

        public ApiResult UserCreate()
        {
            try
            {
                #region "connectionString Validation"
                if (String.IsNullOrWhiteSpace(connectionString))
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, $"La cadena de conexión está vacía. Seg: {nameof(UserCreate)}");
                }
                #endregion

                Dictionary<string, object> parameters = new()
                {
                     { "?user_type_id", user.UserTypeId }
                    ,{ "?names", user.Names ?? String.Empty  }
                    ,{ "?lastnames", user.Lastnames ?? String.Empty  }
                    ,{ "?email", user.Email ?? String.Empty }
                    ,{ "?password", user.Password ?? String.Empty }
                    ,{ "?env", user.Environment ?? String.Empty }
                    ,{ "?state", user.StateId }
                    ,{ "?user_id_add", user.UserAdd }
                    ,{ "?date_add", user.DateAdd }
                };

                ApiResult apiResult = new ColmenaMYSQLConnector(connectionString, query, parameters, config.pwdAES)
                    .InsertQuery();

                if(apiResult.code == StateOperation.OK && apiResult.oDyn != null)
                {
                    user.UserId = unchecked((int)(long)apiResult.oDyn);
                    apiResult.oDyn = user;
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

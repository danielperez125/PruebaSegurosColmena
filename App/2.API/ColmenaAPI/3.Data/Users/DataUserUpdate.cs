using Entities.Colmena.NuGet;
using MYSQLConnector.Colmena.NuGet;
using Transversal.Entities.Colmena;
using Utils.Colmena.NuGet.Response;

namespace Data.Users.Colmena
{
    public class DataUserUpdate
    {
        #region  "Connection String"
        private readonly string? connectionString = (!String.IsNullOrWhiteSpace(config.settings?.connection?.ConnectionString) ? config.settings.connection.ConnectionString : null);
        #endregion

        private readonly User user;
        public DataUserUpdate(User user) => this.user = user;

        private const string query =
            @"
                UPDATE users
                SET  user_type_id       = IFNULL(?user_type_id, user_type_id)
                    ,names              = IFNULL(?names, names)
                    ,lastnames          = IFNULL(?lastnames, lastnames)
                    ,email              = IFNULL(?email, email)
                    ,password           = IFNULL(?password, password)
                    ,env                = IFNULL(?env, env)
                    ,state              = IFNULL(?state, state)
                    ,user_id_mod        = IFNULL(?user_id_mod, user_id_mod)
                    ,date_mod           = IFNULL(?date_mod, date_mod)
                WHERE user_id           = ?user_id;
                SELECT ROW_COUNT();
            ";

        public ApiResult UserUpdate()
        {
            try
            {
                #region "connectionString Validation"
                if (String.IsNullOrWhiteSpace(connectionString))
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, $"La cadena de conexión está vacía. Seg: {nameof(UserUpdate)}");
                }
                #endregion

                Dictionary<string, object> parameters = new ()
                {
                     { "?user_id", user.UserId }
                    ,{ "?user_type_id", user.UserTypeId > 0 ? user.UserTypeId                       : DBNull.Value }
                    ,{ "?names", !String.IsNullOrWhiteSpace(user.Names) ? user.Names                : DBNull.Value }
                    ,{ "?lastnames", !String.IsNullOrWhiteSpace(user.Lastnames) ? user.Lastnames    : DBNull.Value }
                    ,{ "?email", !String.IsNullOrWhiteSpace(user.Email) ? user.Email                : DBNull.Value }
                    ,{ "?password", !String.IsNullOrWhiteSpace(user.Password) ? user.Password       : DBNull.Value }
                    ,{ "?env", !String.IsNullOrWhiteSpace(user.Environment) ? user.Environment      : DBNull.Value }
                    ,{ "?state", user.StateId > 0 ? user.StateId                                    : DBNull.Value }
                    ,{ "?user_id_mod", user.UserMod != null ? user.UserMod                          : DBNull.Value }
                    ,{ "?date_mod", user.DateMod != null ? user.DateMod                             : DBNull.Value }
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

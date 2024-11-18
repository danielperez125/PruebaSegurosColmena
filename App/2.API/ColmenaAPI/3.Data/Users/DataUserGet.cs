using Utils.Colmena.NuGet.Response;
using Transversal.Entities.Colmena;
using Entities.Colmena.NuGet;
using Data.Users.Colmena.Mappers;
using MYSQLConnector.Colmena.NuGet;

namespace Data.Users.Colmena
{
    public class DataUserGet
    {
        #region  "Connection String"
        private readonly string? connectionString = (!String.IsNullOrWhiteSpace(config.settings?.connection?.ConnectionString) ? config.settings.connection.ConnectionString : null);
        #endregion

        private readonly User user = new();
        private readonly string? likeFilter;
        public DataUserGet(User user) => this.user = user;
        public DataUserGet() { }
        public DataUserGet(string? likeFilter) => this.likeFilter = likeFilter;


        private const string query =
            @"
                    SELECT  user.user_id
                           ,user.user_type_id
                           ,ustp.name
                           ,user.names
                           ,user.lastnames
                           ,user.email
                           ,user.env
                           ,user.state
                           ,user.user_id_add
                           ,user.date_add
                           ,user.user_id_mod
                           ,user.date_mod
                    FROM users user
                    INNER JOIN user_types   ustp ON ustp.user_type_id = user.user_type_id
            ";

        public ApiResult UserGet()
        {
            try
            {
                #region "connectionString Validation"
                if (String.IsNullOrWhiteSpace(connectionString))
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, $"La cadena de conexión está vacía. Seg: {nameof(UserGet)}");
                }
                #endregion

                ColmenaMYSQLConnector conn;

                if (user.UserId > 0 || user.UserTypeId > 0 || !String.IsNullOrWhiteSpace(user.Names) || !String.IsNullOrWhiteSpace(user.Lastnames) || !String.IsNullOrWhiteSpace(user.Email))
                {
                    string queryParam = query +
                        @" WHERE (CASE WHEN ?user_id        IS NOT NULL THEN (CASE WHEN user.user_id          = ?user_id         THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND (CASE WHEN ?user_type_id   IS NOT NULL THEN (CASE WHEN ustp.user_type_id     = ?user_type_id    THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND (CASE WHEN ?names          IS NOT NULL THEN (CASE WHEN user.names            = ?names           THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND (CASE WHEN ?lastnames      IS NOT NULL THEN (CASE WHEN user.lastnames        = ?lastnames       THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND (CASE WHEN ?email          IS NOT NULL THEN (CASE WHEN user.email            = ?email           THEN 1 ELSE 0 END) ELSE 1 END)=1
                             AND user.state <> 2
                           LIMIT 500;
                        ";


                    Dictionary<string, object> parameters = new()
                    {
                         { "?user_id", user.UserId > 0 ? user.UserId                                    : DBNull.Value }
                        ,{ "?user_type_id", user.UserTypeId > 0 ? user.UserTypeId                       : DBNull.Value }
                        ,{ "?names", !String.IsNullOrWhiteSpace(user.Names) ? user.Names                : DBNull.Value }
                        ,{ "?lastnames", !String.IsNullOrWhiteSpace(user.Lastnames) ? user.Lastnames    : DBNull.Value }
                        ,{ "?email", !String.IsNullOrWhiteSpace(user.Email) ? user.Email                : DBNull.Value }
                    };

                    conn = new(connectionString, queryParam, parameters, config.pwdAES);
                }
                else if (!String.IsNullOrWhiteSpace(likeFilter))
                {
                    string queryParam = query +
                        @" WHERE user.names     LIKE CONCAT('%', ?likeFilter, '%')
                              OR user.lastnames LIKE CONCAT('%', ?likeFilter, '%')
                              OR user.email     LIKE CONCAT('%', ?likeFilter, '%')
                              AND user.state <> 2
                           LIMIT 500;
                        ";


                    Dictionary<string, object> parameters = new()
                    {
                         { "?likeFilter", likeFilter }
                    };

                    conn = new(connectionString, queryParam, parameters, config.pwdAES);
                }
                else
                {
                    conn = new(connectionString, $"{query} WHERE user.state <> 2 LIMIT 500;", config.pwdAES);
                }


                ApiResult apiResult = conn.GetQuery();

                if (apiResult.oDyn != null && apiResult.code == StateOperation.OK)
                {
                    if (user.UserId > 0 || !String.IsNullOrWhiteSpace(user.Email))
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

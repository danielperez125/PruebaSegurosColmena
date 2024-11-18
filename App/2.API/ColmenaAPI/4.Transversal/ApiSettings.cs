using Entities.Colmena.NuGet.Enums;
using System.ComponentModel;
using Utils.Colmena.NuGet.Response;
using static Entities.Colmena.NuGet.Enums.ColmenaExpiresEnum;

namespace Transversal.Entities.Colmena
{
    [Description("Clase de configuración del API")]
    public class ApiSettings
    {
        public Connection? connection { get; set; }
        public Environment environment { get; set; } = new();
    }
    public class Connection
    {
        public string ConnD { get; set; } = String.Empty;
        public string ConnT { get; set; } = String.Empty;
        public string ConnC { get; set; } = String.Empty;
        public string ConnP { get; set; } = String.Empty;
        public string ConnLogin { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
    }

    public class Environment
    {
        public string Name { get; set; } = String.Empty;
    }

    public static class config
    {
        public static ApiSettings? settings { get; set; }
        public static string pwdAES { get; set; } = "Tc35409bb97a48ed82c3fb1cb5c13a9a";
        public static string sKey { get; set; } = "0tOP2oQLIJ69eSNSgDamucj8TdjrE0PtC17+mg022ZE=|ColmenaIVDecrypt|thItEEz/LKtfKcpuzHDDvg==";
        public static ExpiresToken expiresToken { get; set; } = ExpiresToken.OneMinute;
        public static string ColmenaIVDecrypt { get; set; } = "d3v3NvC073";
        public static EnvironmentEnum Environment { get; set; }
    }

    public static class JwtClaimColmena
    {
        public static long userId { get; set; }
        public static long userTypeId { get; set; }
        public static string issuer { get; set; } = "ColmenaSeguros.©om";
        public static string audience { get; set; } = "ColmenaSeguros.©om";
    }

    public static class ConnectionString
    {
        public static ApiResult Sett(EnvironmentEnum env)
        {
            try
            {
                ApiResult apiResult = new(StateOperation.OK);

                if (config.settings?.connection != null)
                {
                    config.settings.connection.ConnectionString = env switch
                    {
                        EnvironmentEnum.Dev  => config.settings.connection.ConnD,
                        EnvironmentEnum.Test => config.settings.connection.ConnT,
                        EnvironmentEnum.Cert => config.settings.connection.ConnC,
                        EnvironmentEnum.Prod => config.settings.connection.ConnP,
                        _ => String.Empty
                    };

                    if (String.IsNullOrWhiteSpace(config.settings.connection.ConnectionString))
                    {
                        apiResult = new ApiResult(StateOperation.InvalidRequest, true, $"{nameof(config.settings.connection.ConnectionString)} is empty");
                    }
                }
                else
                {
                    apiResult = new ApiResult(StateOperation.InvalidRequest, true, "Env - Connection is null");
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

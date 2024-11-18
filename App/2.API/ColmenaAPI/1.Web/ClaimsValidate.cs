using Entities.Colmena.NuGet.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Transversal.Entities.Colmena;
using Utils.Colmena.NuGet.Response;

namespace Colmena.API
{
    public class ClaimsValidate : Controller
    {
        public static void Validate(HttpContext httpContext)
        {
            try
            {
                if (httpContext == null)
                    throw new Exception("httpContext IS NULL");

                if (httpContext.User.Identity is not ClaimsIdentity identity)
                    throw new Exception("ClaimsIdentity IS NULL");

                List<string> validations = new();

                long uId = 0;
                long uTpId = 0;
                EnvironmentEnum env;

                var _userId = identity.FindFirst("UserId")?.Value;
                if (string.IsNullOrWhiteSpace(_userId) || !long.TryParse(_userId, out uId))
                {
                    validations.Add("UserId");
                }
                if (uId == 0)
                {
                    validations.Add("uId == 0");
                }

                var _userTypeId = identity.FindFirst("UserTypeId")?.Value;
                if (string.IsNullOrWhiteSpace(_userTypeId) || !long.TryParse(_userTypeId, out uTpId))
                {
                    validations.Add("UserTypeId");
                }
                if (uTpId == 0)
                {
                    validations.Add("uTpId == 0");
                }


                var _env = identity.FindFirst("Env")?.Value;
                if (string.IsNullOrWhiteSpace(_env) || !Enum.TryParse<EnvironmentEnum>(_env, out env))
                {
                    validations.Add("Env");
                }
                else
                {
                    ApiResult apiResult = ConnectionString.Sett(env);

                    if(apiResult.code != StateOperation.OK)
                    {
                        validations.Add("Env - Connection is null");
                    }
                }



                if (!validations.Any())
                {
                    JwtClaimColmena.userId = uId;
                    JwtClaimColmena.userTypeId = uTpId;
                }
                else
                {
                    throw new Exception($"Error Getting Claims. Trace: {string.Join(", ", validations)}");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

using Entities.Colmena.NuGet;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Transversal.Entities.Colmena;
using Utils.Colmena.NuGet.Response;
using static Entities.Colmena.NuGet.Enums.ColmenaExpiresEnum;

namespace JwtAuth.Colmena.NuGet
{
    public class JwtLogin
    {
        public static ApiResult GenerateJSONWebToken(User user, string sKey, ExpiresToken expiresToken)
        {
            try
            {
                if(String.IsNullOrWhiteSpace(user.Email) || String.IsNullOrWhiteSpace(sKey))
                    return new ApiResult(StateOperation.InvalidRequest, true, $"Campos obligatorios: user.{nameof(user.Email)}, {nameof(sKey)}");


                byte[] bytes = Encoding.ASCII.GetBytes(sKey);
                SecurityTokenDescriptor securityTokenDescriptor = new()
                {
                    Subject = new ClaimsIdentity((IEnumerable<Claim>)new Claim[6]
                    {
                        new("UserId", user.UserId.ToString()),
                        new("UserTypeId", user.UserTypeId.ToString()),
                        new("Env", user.Environment ?? string.Empty),
                        new(JwtRegisteredClaimNames.Sub, user.Email ?? String.Empty),
                        new(JwtRegisteredClaimNames.Email, user.Email ?? String.Empty),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    }),
                    Expires = new DateTime?(DateTime.UtcNow.AddMinutes(Convert.ToDouble((int)expiresToken))),
                    Issuer = JwtClaimColmena.issuer,
                    Audience = JwtClaimColmena.audience,
                    SigningCredentials = new SigningCredentials((SecurityKey)new SymmetricSecurityKey(bytes), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512")
                };

                JwtSecurityTokenHandler securityTokenHandler = new();
                string str = ((SecurityTokenHandler)securityTokenHandler).WriteToken(((SecurityTokenHandler)securityTokenHandler).CreateToken(securityTokenDescriptor));

                if (!string.IsNullOrWhiteSpace(str))
                {
                    return new ApiResult(StateOperation.OK, str);
                }

                return new ApiResult(StateOperation.InternalServerError, isValidation: true, "Error no controlado. No se generó el WebToken");
            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }
    }
}

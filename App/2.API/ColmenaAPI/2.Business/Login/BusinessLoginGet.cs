using Data.Login.Colmena;
using Entities.Colmena.NuGet;
using JwtAuth.Colmena.NuGet;
using System.Text;
using Transversal.Entities.Colmena;
using Utils.Colmena.NuGet.CipherAES256;
using Utils.Colmena.NuGet.Response;

namespace Business.Login.Colmena
{
    public class BusinessLoginGet
    {
        private readonly string userName;
        private string password;
        public BusinessLoginGet(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        public ApiResult Validate()
        {
            try
            {
                List<string> validations = new();

                if (String.IsNullOrWhiteSpace(userName))
                {
                    validations.Add($" {nameof(userName)} incorrecto");
                }
                if (String.IsNullOrWhiteSpace(password))
                {
                    validations.Add($" {nameof(password)} incorrecto");
                }


                if (validations.Any())
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, String.Join(" - ", validations));
                }
                else
                {
                    return Process();
                }
            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }

        private ApiResult Process()
        {
            try
            {


                //var ciphPwd = CipherAES.Cipher("123456", config.pwdAES, true);
                //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ciphPwd.oDyn?.ToString() ?? "");
                //var text = System.Convert.ToBase64String(plainTextBytes);

                var pwdDecode = Base64Decode(password);
                ApiResult pwdFRDecrypt = CipherAES.Cipher(pwdDecode, config.pwdAES, false);

                if (pwdFRDecrypt.code != StateOperation.OK || pwdFRDecrypt.oDyn == null)
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, $"Error de inicio de sesión: {pwdFRDecrypt.message}");
                }


                ApiResult apiResult = new DataLoginGet(userName)
                    .LoginGet();

                if (apiResult.code == StateOperation.OK && apiResult.oDyn != null)
                {
                    User user = (User)apiResult.oDyn;

                    string pwdBDdecode = Base64Decode(user.Password ?? String.Empty);

                    user.Password = null;

                    ApiResult apiResPwdBD = CipherAES.Cipher(pwdBDdecode, config.pwdAES, false);

                    if (apiResPwdBD.code != StateOperation.OK || apiResPwdBD.oDyn == null)
                    {
                        return new ApiResult(StateOperation.InvalidRequest, true, $"Error de inicio de sesión: {apiResPwdBD.message}");
                    }

                    string? pwdFR = pwdFRDecrypt.oDyn.ToString();
                    string? pwdBD = apiResPwdBD.oDyn.ToString();

                    if (String.IsNullOrWhiteSpace(pwdFR) || String.IsNullOrWhiteSpace(pwdBD) || pwdFR != pwdBD)
                    {
                        return new ApiResult(StateOperation.InvalidRequest, true, "Usuario o contraseña incorrecta");
                    }


                    apiResult = JwtLogin.GenerateJSONWebToken(user, config.sKey, config.expiresToken);
                    //apiResult.message = apiResult.oDyn?.ToString();
                }
                else
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, "Usuario o contraseña incorrecta");
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}


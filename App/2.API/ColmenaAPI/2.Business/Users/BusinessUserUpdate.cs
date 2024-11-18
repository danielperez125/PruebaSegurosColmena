using Utils.Colmena.NuGet.Response;
using Transversal.Entities.Colmena;
using Entities.Colmena.NuGet;
using Data.Users.Colmena;

namespace Business.Users.Colmena
{
    public class BusinessUserUpdate
    {
        private readonly User user;
        public BusinessUserUpdate(User user) => this.user = user;
        
        public ApiResult Validate()
        {
            try
            {
                List<string> validations = new();

                if (user.UserId == 0)
                {
                    validations.Add($" {nameof(user.UserId)} es requerido");
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
                List<string> validations = new();
                List<string?> exceptions = new();
                ApiResult result;

                result = new DataUserGet(new User { UserId = user.UserId})
                    .UserGet();
                if (result.code == StateOperation.NoContent)
                {
                    validations.Add($"{nameof(user.UserId)} no existe");
                }
                else if (result.code == StateOperation.InvalidRequest)
                {
                    validations.Add(result.message ?? $"Error ejecutando {nameof(DataUserGet)}");
                }
                else if (result.code == StateOperation.InternalServerError)
                {
                    exceptions.Add(result.message);
                }


                if (!String.IsNullOrWhiteSpace(user.Email))
                {
                    result = new DataUserGet(new User { Email = user.Email })
                        .UserGet();

                    if (result.code == StateOperation.OK)
                    {
                        validations.Add($"Ya está registrado el {nameof(user.Email)} {user.Email}");
                    }
                    else if (result.code == StateOperation.InvalidRequest)
                    {
                        validations.Add(result.message ?? $"Error ejecutando {nameof(DataUserGet)}");
                    }
                    else if (result.code == StateOperation.InternalServerError)
                    {
                        exceptions.Add(result.message);
                    }
                }




                if (!validations.Any() && !exceptions.Any())
                {
                    user.UserMod = JwtClaimColmena.userId;
                    user.DateMod = DateTime.Now;

                    ApiResult apiResult = new DataUserUpdate(user)
                        .UserUpdate();

                    return apiResult;
                }
                else if (validations.Any())
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, String.Join(" - ", validations));
                }
                else
                {
                    return new ApiResult(StateOperation.InternalServerError, true, String.Join(" - ", exceptions));
                }

            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }
    }
}
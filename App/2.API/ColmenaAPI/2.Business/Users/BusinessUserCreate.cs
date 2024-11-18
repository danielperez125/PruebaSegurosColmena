using Utils.Colmena.NuGet.Response;
using Transversal.Entities.Colmena;
using Entities.Colmena.NuGet;
using Data.UserTypes.Colmena;
using Data.Users.Colmena;
using static Entities.Colmena.NuGet.Enums.ColmenaStateEnum;
using static Entities.Colmena.NuGet.Enums.ColmenaUserEnum;
using Entities.Colmena.NuGet.Enums;

namespace Business.Users.Colmena
{
    public class BusinessUserCreate
    {
        private readonly User user;
        public BusinessUserCreate(User user) => this.user = user;

        public ApiResult Validate()
        {
            try
            {
                ConnectionString.Sett(EnvironmentEnum.Dev);
                JwtClaimColmena.userId = (int)UserEnum.SuperAdmin;

                List<string> validations = new();

                if (user.UserTypeId == 0)
                {
                    validations.Add($"{nameof(user.UserTypeId)} requerido");
                }
                if (String.IsNullOrWhiteSpace(user.Names))
                {
                    validations.Add($"{nameof(user.Names)} requerido");
                }
                if (String.IsNullOrWhiteSpace(user.Lastnames))
                {
                    validations.Add($"{nameof(user.Lastnames)} requerido");
                }
                if (String.IsNullOrWhiteSpace(user.Email))
                {
                    validations.Add($"{nameof(user.Email)} requerido");
                }
                if (String.IsNullOrWhiteSpace(user.Password))
                {
                    validations.Add($"{nameof(user.Password)} requerido");
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

                result = new DataUserTypeGet(userTypeId: user.UserTypeId)
                    .UserTypeGet();

                if (result.code == StateOperation.NoContent)
                {
                    validations.Add($" {nameof(user.UserTypeId)} incorrecto");
                }
                else if (result.code == StateOperation.InvalidRequest)
                {
                    validations.Add(result.message ?? $"Error ejecutando {nameof(DataUserTypeGet)}");
                }
                else if (result.code == StateOperation.InternalServerError)
                {
                    exceptions.Add(result.message);
                }

                result = new DataUserGet(new User() { Email = user.Email })
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



                if (!validations.Any() && !exceptions.Any())
                {
                    user.Environment = user.Environment ?? EnvironmentEnum.Test.ToString();
                    user.StateId = (int)State.Activo;
                    user.UserAdd = JwtClaimColmena.userId;
                    user.DateAdd = DateTime.Now;

                    ApiResult apiResult = new DataUserCreate(user)
                        .UserCreate();

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
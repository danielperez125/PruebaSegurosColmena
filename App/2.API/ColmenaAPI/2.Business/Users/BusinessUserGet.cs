using Data.Users.Colmena;
using Entities.Colmena.NuGet;
using Utils.Colmena.NuGet.Response;

namespace Business.Users.Colmena
{
    public class BusinessUserGet
    {
        private readonly int? userId;
        private readonly int? userTypeId;
        private readonly string? names;
        private readonly string? lastNames;
        private readonly string? email;
        public BusinessUserGet(int? userId, int? userTypeId, string? names, string? lastNames, string? email)
        {
            this.userId = userId;
            this.userTypeId = userTypeId;
            this.names = names;
            this.lastNames = lastNames;
            this.email = email;
        }

        public ApiResult Validate()
        {
            try
            {
                List<string> validations = new();

                if ((userId == null || userId == 0) && (userTypeId == null || userTypeId == 0) && (String.IsNullOrWhiteSpace(names) 
                    && String.IsNullOrWhiteSpace(lastNames) && String.IsNullOrWhiteSpace(email)))
                {
                    validations.Add(" Se requiere alguno de los siguientes parámetros para esta búsqueda: " +
                        $"{nameof(userId)}, {nameof(userTypeId)}, {nameof(names)}, {nameof(lastNames)}, {nameof(email)}");
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
                User user = new()
                {
                    UserId = userId ?? 0,
                    UserTypeId = userTypeId ?? 0,
                    Names = names,
                    Lastnames = lastNames,
                    Email = email
                };

                ApiResult apiResult = new DataUserGet(user)
                    .UserGet();

                return apiResult;
            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }
    }
}


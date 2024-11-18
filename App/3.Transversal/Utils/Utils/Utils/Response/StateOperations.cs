using System.ComponentModel;

namespace Utils.Colmena.NuGet.Response
{
    [Description("Based in Microsoft.AspNetCore.Http.StatusCodes")]
    public enum StateOperation
    {
        OK = 200,
        NoContent = 204,
        Unauthorized = 401,
        InvalidRequest = 409,
        InternalServerError = 500
    }
}
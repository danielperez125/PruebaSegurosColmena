namespace Utils.Colmena.NuGet.Response
{
    public class ApiResult
    {
        public ApiResult(StateOperation code)
        {
            this.code = code;
            this.message = Enum.GetName(typeof(StateOperation), (int)code);
        }
        public ApiResult(StateOperation code, object oDyn)
        {
            this.code = code;
            this.message = Enum.GetName(typeof(StateOperation), (int)code);
            this.oDyn = oDyn;
        }

        public ApiResult(StateOperation code, bool isValidation, string validationMssg)
        {
            this.code = code;
            if (isValidation)
            {
                this.message = $"{Enum.GetName(typeof(StateOperation), (int)code)} - {validationMssg}";
            }
        }
        public ApiResult(StateOperation code, Exception ex)
        {
            this.code = code;
            this.message = $"{Enum.GetName(typeof(StateOperation), (int)code)} - {ex.Message}";
        }

        public StateOperation code { get; set; }
        public string? message { get; set; }
        public object? oDyn { get; set; }
    }
}

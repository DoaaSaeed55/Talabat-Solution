namespace Talabat.APIs.Errors
{
    public class ApiValidationErrorsResponse :ApiResponse
    {

        public IEnumerable<string> Errors { get; set; }

        public ApiValidationErrorsResponse() : base(statusCode:400)
        {

        }

    }
}

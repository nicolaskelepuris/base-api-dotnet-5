using Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        public IActionResult Error(int statusCode)
        {
            var response = new ApiResponse<string>(data: null, error: GetDefaultMessageForStatusCode(statusCode));
            
            return StatusCode(statusCode, response);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad request",
                401 => "Not authorized",
                403 => "Forbidden",
                404 => "No content found",
                500 => "Internal error",
                _ => null
            };
        }
    }
}
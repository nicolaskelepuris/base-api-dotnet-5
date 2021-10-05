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
            return StatusCode(statusCode, new ApiResponse<string>
            {
                Success = false,
                Data = null,
                Error = GetDefaultMessageForStatusCode(statusCode)
            });
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
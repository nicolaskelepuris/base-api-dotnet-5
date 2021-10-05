using System;
using System.Threading.Tasks;
using Application.Responses;
using Application.Responses.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<IActionResult> CreateResponse<T>(Func<Task<T>> function) where T : BaseResponse
        {
            var data = await function();

            var statusCode = data.StatusCode ?? 200;

            var hasError = !String.IsNullOrEmpty(data.ErrorMessage);

            var dataToReturn = hasError ? null : data;

            return StatusCode(statusCode, new ApiResponse<T>(data: dataToReturn, error: data.ErrorMessage));
        }
    }
}
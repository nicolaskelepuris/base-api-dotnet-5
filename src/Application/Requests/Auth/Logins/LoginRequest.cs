using Application.Responses.Auth.Logins;
using MediatR;

namespace Application.Requests.Auth.Logins
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
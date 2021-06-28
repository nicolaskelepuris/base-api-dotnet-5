using Application.Responses.Auth.Logins;
using MediatR;

namespace Application.Requests.Auth.Logins
{
    public class LoginGoogleRequest : IRequest<LoginResponse>
    {
        public string Token { get; set; }
    }
}
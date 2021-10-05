using Application.Responses.Auth.Logins;
using FluentValidation;
using MediatR;

namespace Application.Requests.Auth.Logins
{
    public class LoginGoogleRequest : IRequest<LoginResponse>
    {
        public string Token { get; set; }
    }

    public class LoginGoogleRequestValidator : AbstractValidator<LoginGoogleRequest>
    {
        public LoginGoogleRequestValidator()
        {
            RuleFor(p => p.Token).NotEmpty();
        }
    }
}
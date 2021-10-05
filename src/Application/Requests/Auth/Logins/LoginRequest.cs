using Application.Responses.Auth.Logins;
using FluentValidation;
using MediatR;

namespace Application.Requests.Auth.Logins
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(p => p.Email).NotEmpty();
            RuleFor(p => p.Password).NotEmpty();
        }
    }
}
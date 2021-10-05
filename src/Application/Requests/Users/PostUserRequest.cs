using Application.Responses.Users;
using System.Linq;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;

namespace Application.Requests.Users
{
    public class PostUserRequest : IRequest<UserResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Picture { get; set; }
    }

    public class PostUserRequestValidator : AbstractValidator<PostUserRequest>
    {
        public PostUserRequestValidator(UserManager<AppUser> userManager)
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Name).Must(name => !name.Any(c => !Char.IsLetter(c) || !Char.IsWhiteSpace(c)));

            RuleFor(p => p.Password).NotEmpty();
            var passwordValidator = new PasswordValidator<AppUser>();
            RuleFor(p => p.Password)
                .Must(password => passwordValidator.ValidateAsync(userManager, null, password).Result == IdentityResult.Success)
                .WithMessage("Senha invalida")
                .When(p => !string.IsNullOrEmpty(p.Password));

            RuleFor(p => p.Email).NotEmpty();
            RuleFor(p => p.Email).EmailAddress();
            RuleFor(p => p.Email)
                .Must(email => userManager.FindByEmailAsync(email).Result == null)
                .WithMessage("Email ja cadastrado")
                .When(p => !string.IsNullOrEmpty(p.Email));
        }
    }
}
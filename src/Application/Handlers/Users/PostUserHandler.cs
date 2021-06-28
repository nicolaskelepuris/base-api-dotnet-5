using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Users;
using Application.Responses.Users;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers.Users
{
    public class PostUserHandler : IRequestHandler<PostUserRequest, UserResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public PostUserHandler(UserManager<AppUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserResponse> Handle(PostUserRequest request, CancellationToken cancellationToken)
        {
            var requestValid = await IsRequestValidAsync(request);

            if (!requestValid)
            {
                return new UserResponse() { StatusCode = 400, ErrorMessage = "Email ou senha invalidos" };
            }

            var user = _mapper.Map<AppUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result != IdentityResult.Success)
            {
                return new UserResponse() { StatusCode = 400, ErrorMessage = "Falha ao criar usuario no banco" };
            }

            return _mapper.Map<UserResponse>(user);
        }

        private async Task<bool> IsRequestValidAsync(PostUserRequest request)
        {
            return await IsEmailValidAsync(request.Email) && await IsPasswordValidAsync(request.Password);
        }

        private async Task<bool> IsPasswordValidAsync(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return false;
            }
            var passwordValidator = new PasswordValidator<AppUser>();

            var result = await passwordValidator.ValidateAsync(_userManager, null, password);

            if (result != IdentityResult.Success)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> IsEmailValidAsync(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return false;
            }

            try
            {
                var mailAddress = new MailAddress(email);
                if (mailAddress.Address != email)
                {
                    return false;
                }
            }
            catch (FormatException)
            {
                return false;
            }

            var userSameEmail = await _userManager.FindByEmailAsync(email);

            if (userSameEmail != null)
            {
                return false;
            }

            return true;
        }
    }
}
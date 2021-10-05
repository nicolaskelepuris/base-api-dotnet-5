using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Auth.Logins;
using Application.Responses.Auth.Logins;
using Application.Responses.Users;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers.Auth.Logins
{
    public class LoginGoogleHandler : IRequestHandler<LoginGoogleRequest, LoginResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public LoginGoogleHandler(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<LoginResponse> Handle(LoginGoogleRequest request, CancellationToken cancellationToken)
        {
            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);

                if (!payload.EmailVerified)
                {
                    return (LoginResponse) LoginResponse.WithError(statusCode: 200, error: "Email nao confirmado");
                }
            }
            catch (Exception)
            {
                return (LoginResponse) LoginResponse.WithError(statusCode: 200, error: "Erro ao ler dados do usuario via Google");
            }

            var user = await _userManager.FindByEmailAsync(payload.Email);

            IdentityResult result;
            bool userExists = user != null;

            if (userExists)
            {
                _mapper.Map<GoogleJsonWebSignature.Payload, AppUser>(payload, user);
                result = await _userManager.UpdateAsync(user);
            }
            else
            {
                user = _mapper.Map<AppUser>(payload);

                result = await _userManager.CreateAsync(user);
            }

            if (!result.Succeeded)
            {
                return (LoginResponse) LoginResponse.WithError(statusCode: 200, error: "Erro ao criar/atualizar usuario");
            }


            return new LoginResponse()
            {
                User = _mapper.Map<UserResponse>(user),
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
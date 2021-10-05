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
            var user = _mapper.Map<AppUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result != IdentityResult.Success)
            {
                return (UserResponse) UserResponse.WithError(statusCode: 200, error: "Falha ao criar usuario");
            }

            return _mapper.Map<UserResponse>(user);
        }
    }
}
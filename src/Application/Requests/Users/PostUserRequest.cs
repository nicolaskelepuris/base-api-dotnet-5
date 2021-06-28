using Application.Responses.Users;
using MediatR;

namespace Application.Requests.Users
{
    public class PostUserRequest : IRequest<UserResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
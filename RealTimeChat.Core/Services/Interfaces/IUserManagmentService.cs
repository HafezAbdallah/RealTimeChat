

using RealTimeChat.Core.Dtos;

namespace RealTimeChat.Core.Services.Interfaces
{
    public interface IUserManagmentService
    {
        public void Login(LoginRequest request);
    }
}

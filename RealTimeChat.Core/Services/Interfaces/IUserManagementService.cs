using RealTimeChat.Core.Dtos;
using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Services.Interfaces
{
    public interface IUserManagementService
    {
        Task<bool> Login(LoginRequest request);

        Task<bool> Register(User user);

        Task<List<UserStatusDto>> GetUsersStatus();

        Task SaveUserMessage(string usernameTo, string usernameFrom, string messageContent);

        Task<Dictionary<string, List<ChatMessage>>> GetUnRecievedMessages(string username);  
    }
}

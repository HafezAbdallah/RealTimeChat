
using RealTimeChat.Core.Dtos;
using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repos
{
    public interface IUsermanagementRepo
    {
        Task<bool> DoesUserExist(LoginRequest userInfo);
        Task<bool> Register(User user);
        Task<List<string>> GetUsersNames();
        Task SaveUserMessage(Message message);
        Task <User> GetUser (string username);
        Task<List<Message>> GetUsersUnRecivedMessages(string username);// this should be in MessagesRepo but it's here for simplicty
    }
}

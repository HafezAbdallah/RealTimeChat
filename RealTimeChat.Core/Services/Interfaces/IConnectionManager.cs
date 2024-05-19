
namespace RealTimeChat.Core.Services.Interfaces
{
    public interface IConnectionManager
    {
        bool AddUser(string connectionId, string username);
        bool RemoveUser(string username);
        string GetUserConnection(string username);
        List<string> GetOnlineUsersNames();

    }
}

using RealTimeChat.Core.Services.Interfaces;
using System.Collections.Concurrent;

namespace RealTimeChat.Core.Services.Implmentations
{
    public class ConnectionManager : IConnectionManager
    {
        private ConcurrentDictionary<string, string> _userNames { get; } = new ConcurrentDictionary<string, string>();

        public bool AddUser(string connectionId, string username)
        {
            return _userNames.TryAdd(username, connectionId); // we can have a list of connections if the user will have more than one connection
        }
        public bool RemoveUser(string userName)
        {
            return _userNames.TryRemove(userName, out string connectionId);
        }

        public List<string> GetOnlineUsersNames()
        {
            return _userNames.Select(x=>x.Key).ToList();

        }

        public string GetUserConnection(string username)
        {
            _userNames.TryGetValue(username, out string connection);
            return connection;
        }

      
    }
}

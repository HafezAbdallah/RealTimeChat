using Microsoft.AspNetCore.SignalR;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Services.Interfaces;


namespace RealTimeChat.Hubs
{
    public class ChatHub : Hub
    {
        public readonly IConnectionManager _connectionManager;
        public readonly IUserManagementService _userManagementService;
        public ChatHub(IConnectionManager connectionManager, IUserManagementService userManagementService)
        {
            _connectionManager = connectionManager;
            _userManagementService = userManagementService;
        }
        public async Task SendMessage(string username, string message)
        {
            var callerUserName = Context.GetHttpContext().Request.Cookies["username"];

            var userConnectionId = _connectionManager.GetUserConnection(username);
            if (userConnectionId == null)
            {
                await _userManagementService.SaveUserMessage(username, callerUserName,message);
            }
            else
                await Clients.Client(userConnectionId).SendAsync("ReceiveMessage", callerUserName, message);
        }

        public override async Task OnConnectedAsync()
        {

            var username = Context.GetHttpContext().Request.Cookies["username"];

            _connectionManager.AddUser(Context.ConnectionId, username);

            await Clients.Others.SendAsync("UserConnected", username);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            var username = Context.GetHttpContext().Request.Cookies["username"];
            _connectionManager.RemoveUser(username);

            await Clients.Others.SendAsync("UserDisconnected", username);

            await base.OnDisconnectedAsync(exception);
        }
    }
}

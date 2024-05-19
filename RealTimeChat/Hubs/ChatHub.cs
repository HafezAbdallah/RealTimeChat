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
            var callerUserName = Context.GetHttpContext().Request.Cookies["userName"];

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

            var userName = Context.GetHttpContext().Request.Cookies["userName"];

            _connectionManager.AddUser(Context.ConnectionId, userName);
            var t= await _userManagementService.GetUnRecievedMessages(userName);
            await Clients.Others.SendAsync("UserConnected", userName);


            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            var userName = Context.GetHttpContext().Request.Cookies["userName"];
            _connectionManager.RemoveUser(userName);

            await Clients.Others.SendAsync("UserDisconnected", userName);

            await base.OnDisconnectedAsync(exception);
        }
    }
}

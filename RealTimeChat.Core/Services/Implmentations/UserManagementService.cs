using RealTimeChat.Core.Dtos;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Repos;
using RealTimeChat.Core.Services.Interfaces;

namespace RealTimeChat.Core.Services.Implmentations
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUsermanagementRepo _userManagementRepo;
        private readonly IConnectionManager _connectionManager;

        public UserManagementService(IUsermanagementRepo userManagementRepo, IConnectionManager connectionManager)
        {
            _userManagementRepo = userManagementRepo;
            _connectionManager = connectionManager;

        }

        public async Task<List<UserStatusDto>> GetUsersStatus()
        {
            var allUsersNames = await _userManagementRepo.GetUsersNames();
            var userStatus = new List<UserStatusDto>();
            foreach (var user in allUsersNames)
            {
                userStatus.Add(new UserStatusDto
                {
                    Username = user,
                    Status = string.IsNullOrEmpty(_connectionManager.GetUserConnection(user)) ? UserStatus.Offline : UserStatus.Online

                });
            }
            return userStatus.OrderBy(x => x.Status).ToList();
        }

        public async Task<bool> Login(LoginRequest request)
        {
            //Validate any business logic 

            if (await _userManagementRepo.DoesUserExist(request))
                return true;
            else
                return false;

        }

        public async Task<bool> Register(User user)
        {
            if (await _userManagementRepo.Register(user))
                return true;

            return false;
        }

        public async Task SaveUserMessage(string usernameTo, string usernameFrom, string messageContent)
        {
            var userTo = await _userManagementRepo.GetUser(usernameTo);
            var userFrom = await _userManagementRepo.GetUser(usernameFrom);
            var message = new Message
            {
                Content = messageContent,
                CreatedOn = DateTime.Now,
                UserFrom = userFrom,
                UserTo = userTo,
                
                
            };
            await _userManagementRepo.SaveUserMessage(message);
        }

        public async Task<Dictionary<string, List<ChatMessage>>> GetUnRecievedMessages(string callerUserName)
        {
            Dictionary<string, List<ChatMessage>> result = new Dictionary<string, List<ChatMessage>>();

            var allUsers = (await GetUsersStatus()).Where(x => x.Username != callerUserName).ToList();

            foreach (var user in allUsers)
            {
                result.Add(user.Username, new List<ChatMessage>());
            }

            List<Message> unReceivedMessages = await _userManagementRepo.GetUsersUnRecivedMessages(callerUserName);
            unReceivedMessages.ForEach(x =>
            {
                var userFromName = x.UserFrom.Username;
                result[userFromName].Add(new ChatMessage { Message = x.Content, Type = 1 }); //TODO: create enum for type 1 for recived 0 for sent
            }
                );
            return result;
        }
    }
}

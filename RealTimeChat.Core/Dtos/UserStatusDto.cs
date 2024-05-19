
namespace RealTimeChat.Core.Dtos
{
    public class UserStatusDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public UserStatus Status { get; set; }

    }

    public enum UserStatus
    {
        Online,
        Offline
    }

}



namespace RealTimeChat.Core.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public User UserFrom { get; set; }
        public int UserFromId { get; set; }
        public int UserToId { get; set; }
        public User UserTo { get; set; }
    }

}

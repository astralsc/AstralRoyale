using System;

namespace ClashRoyale.Logic.Home
{
    public class InboxMessage
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }
        public string IconUrl { get; set; }
        public DateTime Timestamp { get; set; }
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public bool IsClanMail { get; set; }
    }
}
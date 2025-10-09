using ClashRoyale.Logic;
using ClashRoyale.Protocol.Messages.Server;

namespace ClashRoyale.Protocol.Messages.Server
{
    public class CancelMatchmakeDoneMessage : PiranhaMessage
    {
        public CancelMatchmakeDoneMessage(Device device) : base(device)
        {
            Id = 24125;
        }
    }
}
using ClashRoyale.Logic;
using ClashRoyale.Protocol.Messages.Server;

namespace ClashRoyale.Protocol.Messages.Server
{
    public class CancelChallengeDoneMessage : PiranhaMessage
    {
        public CancelChallengeDoneMessage(Device device) : base(device)
        {
            Id = 24124;
        }
    }
}
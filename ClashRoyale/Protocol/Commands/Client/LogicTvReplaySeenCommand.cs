using ClashRoyale.Logic;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol.Commands.Client
{
    public class LogicTvReplaySeenCommand : LogicCommand
    {
        public LogicTvReplaySeenCommand(Device device, IByteBuffer buffer) : base(device, buffer)
        {       
        }

        public override void Decode()
        {
            Reader.ReadVInt();
            Reader.ReadVInt();
        }
    }
}

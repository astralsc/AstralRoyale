using ClashRoyale.Logic;
using ClashRoyale.Protocol.Messages.Server;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol.Messages.Client.Tournament
{
    public class AskForCancelTournamentMatchmake : PiranhaMessage
    {
        public AskForCancelTournamentMatchmake(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Id = 14111;
        }

        public override async void Process()
        {
            await new CancelTournamentMatchmake(Device).SendAsync();
        }
    }
}
using System;
using System.Threading.Tasks;
using ClashRoyale.Logic;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Protocol.Commands.Client;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol.Messages.Server
{
    public class CancelTournamentMatchmake : PiranhaMessage
    {
        public CancelTournamentMatchmake(Device device) : base(device)
        {
            Process();
        }

        public async Task Process()
        {
            await new MatchmakeInfoMessage(Device)
            {
                EstimatedDuration = 0 // seconds (300 = 5 min)
            }.SendAsync();
            await Task.Delay(500);
            await new CancelMatchmakeDoneMessage(Device).SendAsync();
        }
    }
}
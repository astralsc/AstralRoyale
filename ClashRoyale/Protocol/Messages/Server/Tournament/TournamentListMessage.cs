using System;
using System.Threading.Tasks;
using ClashRoyale.Logic;
using ClashRoyale.Utilities.Netty;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Protocol.Commands.Client;

namespace ClashRoyale.Protocol.Messages.Server
{
    public class TournamentListMessage : PiranhaMessage
    {
        public TournamentListMessage(Device device) : base(device)
        {
            Id = 26101;
            Encode();
        }

        public async Task Encode()
        {
            Writer.WriteVInt(0);
        }
    }
}
using ClashRoyale.Logic;
using ClashRoyale.Logic.Clan;
using ClashRoyale.Logic.Clan.StreamEntry.Entries;
using ClashRoyale.Protocol.Commands.Server;
using ClashRoyale.Protocol.Messages.Server;
using DotNetty.Buffers;
using ClashRoyale.Utilities.Netty;
using ClashRoyale.Core.Cluster.Protocol.Messages.Server;

namespace ClashRoyale.Protocol.Messages.Client.Alliance
{
    public class DonateAllianceUnitMessage : PiranhaMessage
    {
        public DonateAllianceUnitMessage(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Id = 14310;
        }

        public override async void Process()
        {
            await new ServerErrorMessage(Device)
            {
                Message = "Not implemented yet."
            }.SendAsync();
            return;

            // TODO: Donating Troops/Spells
        }
    }
}
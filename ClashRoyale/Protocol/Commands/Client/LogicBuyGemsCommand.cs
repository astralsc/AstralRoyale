using System;
using System.Threading.Tasks;
using ClashRoyale.Logic;
using ClashRoyale.Files;
using ClashRoyale.Files.CsvLogic;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol
{
    internal class LogicBuyGemsCommand : LogicCommand
    {
        public LogicBuyGemsCommand(Device device, IByteBuffer buffer) : base(device, buffer)
        {
        }

        public int InstanceId { get; set; }

        public override void Decode()
        {
            base.Decode();

            Reader.ReadVInt();
            Reader.ReadVInt();

            Reader.ReadVInt(); // ResourceClassId
            InstanceId = Reader.ReadVInt();
        }
        
        public override async void Process()
        {
            var packs = Csv.Tables.Get(Csv.Files.ResourcePacks).GetDataWithInstanceId<ResourcePacks>(InstanceId);
            var amount = packs.Amount;

            Device.Player.Home.Diamonds += amount;

            await new ServerErrorMessage(Device)
            {
                Message = $"Reward: {amount} Gems (Id={InstanceId})"
            }.SendAsync();
        }
    }
}
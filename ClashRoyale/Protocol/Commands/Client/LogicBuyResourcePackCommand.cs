/*using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;*/

using ClashRoyale.Logic;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol.Commands.Client
{
    public class LogicBuyResourcePackCommand : LogicCommand
    {
        public LogicBuyResourcePackCommand(Device device, IByteBuffer buffer) : base(device, buffer)
        {
        }

        /*public int Amount { get; set; }
        public int ClassId { get; set; }
        public int InstanceId { get; set; }
        public int Index { get; set; }*/

        public override void Decode()
        {
            base.Decode();

            Reader.ReadVInt(); // 0

            Reader.ReadVInt(); // 19
            Reader.ReadVInt(); // 1

            /*Amount = Reader.ReadVInt();
            ClassId = Reader.ReadVInt();
            InstanceId = Reader.ReadVInt();
            Index = Reader.ReadVInt();*/
        }

        public override async void Process()
        {
            await new ServerErrorMessage(Device)
            {
                Message = "Not implemented yet."
            }.SendAsync();

            // Device.Player.Home.Shop.BuyItem(Amount, ClassId, InstanceId, Index);
        }
    }
}
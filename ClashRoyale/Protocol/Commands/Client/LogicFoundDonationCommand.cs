using System;
using System.Threading.Tasks;
using ClashRoyale.Logic;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol
{
    internal class LogicFoundDonationCommand : LogicCommand
    {
        public LogicFoundDonationCommand(Device device, IByteBuffer buffer) : base(device, buffer)
        {
        }
    }
}
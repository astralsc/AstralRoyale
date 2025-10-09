using ClashRoyale.Logic;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol.Messages.Client.Home
{
    public class ReportUserMessage : PiranhaMessage
    {
        public ReportUserMessage(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Id = 10117;
            Process();
        }

        public override async void Process()
        {
            await new ServerErrorMessage(Device)
            {
                Message = "Not implemented yet."
            }.SendAsync();
        }
    }
}
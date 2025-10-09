using System;
using ClashRoyale.Logic;
using DotNetty.Buffers;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;

namespace ClashRoyale.Protocol.Messages.Client.Socials
{
    public class AskForFriendsInviteMessage : PiranhaMessage
    {
        public AskForFriendsInviteMessage(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Id = 10503;
        }

        public override async void Process()
        {
            await new FriendsInviteMessage(this.Device).SendAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClashRoyale.Logic;
using DotNetty.Buffers;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;

namespace ClashRoyale.Protocol.Messages.Client.Socials
{
    public class AskForFriendsListMessage : PiranhaMessage
    {
        public AskForFriendsListMessage(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Id = 10504;

            new FriendsListMessage(Device).SendAsync();
        }
    }
}
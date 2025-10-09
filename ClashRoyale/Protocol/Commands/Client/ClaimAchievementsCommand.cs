﻿using ClashRoyale.Logic;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol
{
    internal class ClaimAchievementsCommand : LogicCommand
    {
        public ClaimAchievementsCommand(Device device, IByteBuffer buffer) : base(device, buffer)
        {
        }

        public override void Decode()
        {
            base.Decode();

            Reader.ReadVInt();//67
            Reader.ReadVInt();//67
            Reader.ReadVInt();//0
            Reader.ReadVInt();//7
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
using System;
using System.Collections.Generic;
using ClashRoyale.Logic;
using ClashRoyale.Database;
using ClashRoyale.Logic.Clan;
using ClashRoyale.Protocol.Messages.Server;
using DotNetty.Buffers;
using System.Linq;

namespace ClashRoyale.Protocol.Messages.Client.Alliance
{
    public class AskForJoinableAlliancesListMessage : PiranhaMessage
    {
        public AskForJoinableAlliancesListMessage(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Id = 14303;
        }

public override async void Process()
{
    try
    {
        List<Logic.Clan.Alliance> allAlliances = await AllianceDb.GetGlobalAlliancesAsync();

        List<Logic.Clan.Alliance> joinableAlliances = allAlliances
            .Where(a => a.Type == 1)
            .ToList();

        await new JoinableAllianceListMessage(Device)
        {
            Alliances = joinableAlliances
        }.SendAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading joinable alliances: {ex}");
    }
}

    }
}
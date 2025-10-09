using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetty.Buffers;
using Newtonsoft.Json;
using SharpRaven.Data;
using ClashRoyale.Logic;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Protocol;
using ClashRoyale.Protocol.Messages.Client.Alliance;
using ClashRoyale.Database;
using ClashRoyale.Extensions;
using ClashRoyale.Logic.Battle;
using ClashRoyale.Logic.Home.StreamEntry;
using ClashRoyale.Utilities.Netty;
using ClashRoyale.Utilities.Utils;
using ClashRoyale.Logic.Home;

public class FriendsListMessage : PiranhaMessage
{
    public List<Friends> Friends { get; set; }

    public FriendsListMessage(Device device) : base(device)
    {
        Id = 20105;

        this.Friends = Device.Player.Home.Friends;
    }


    public override void Encode()
    {
        var packet = this.Writer;
        packet.WriteInt(this.Friends.Count); // Number of friends
        packet.WriteInt(this.Friends.Count); // Number of requests (same)

        foreach (var Friend in this.Friends)
        {
            packet.WriteLong(Friend.PlayerId);                    // Player ID
            packet.WriteBoolean(true);                            // Is online
            packet.WriteLong(Friend.PlayerId);                    // Player ID again
            packet.WriteScString(Friend.Name ?? "");              // Name
            packet.WriteScString(Friend.Facebook?.Identifier ?? ""); // Facebook ID
            packet.WriteScString(Friend.Gamecenter?.Identifier ?? ""); // Gamecenter ID
            packet.WriteVInt(Friend.ExpLevel);                    // Exp Level
            packet.WriteVInt(Friend.Score);                       // Score
            packet.WriteBoolean(false);                           // HasClan
            packet.WriteVInt(Friend.Arena);                       // Arena
            packet.WriteScString(null);                           // Clan Name
            packet.WriteScString(null);                           // Clan Tag
            packet.WriteVInt(-1);                                 // Clan Badge
            packet.WriteInt(0);                                   // Challenge State
            packet.WriteInt(0);                                   // Challenge Wins
        }
    }
}
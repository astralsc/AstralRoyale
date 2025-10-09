using System;
using System.Threading.Tasks;
using ClashRoyale.Logic;
using ClashRoyale.Logic.Home.StreamEntry.Entries;
using ClashRoyale.Protocol.Commands.Client;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol.Commands.Client
{
    public class LogicFriendChallengeCommand : LogicCommand
    {
        public LogicFriendChallengeCommand(Device device, IByteBuffer buffer) : base(device, buffer)
        {
        }

        public string Message { get; set; }
        public int Arena { get; set; }
        public int GameMode { get; set; }

        public override void Decode()
        {
            Message = Reader.ReadScString();
            Reader.ReadBoolean();

            Reader.ReadVInt(); // ClassId
            GameMode = Reader.ReadVInt(); // InstanceId

            Reader.ReadVInt();
            Reader.ReadVInt();

            Reader.ReadVInt();
            Reader.ReadVInt();

            Reader.ReadVInt();

            Arena = Reader.ReadVInt();
        }

        public override async void Process()
        {
            Logger.Log($"Friend Gamemode: {GameMode}", null);

            if (GameMode == 1) // 1 = friend friendly battle
            {
                var home = Device.Player.Home;
                var player = await Resources.Players.GetPlayerAsync(home.Id);

                if (player == null) return;

                var entry = new FriendChallengeStreamEntry
                {
                    Message = Message,
                    Arena = Arena + 1
                };

                entry.SetSender(Device.Player);
                player.AddEntry(entry);
            }
        }
    }
}
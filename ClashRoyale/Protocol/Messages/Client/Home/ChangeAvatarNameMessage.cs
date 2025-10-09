using System;
using System.IO;
using System.Text.RegularExpressions;
using ClashRoyale.Logic;
using ClashRoyale.Protocol.Commands.Server;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol.Messages.Client.Home
{
    public class ChangeAvatarNameMessage : PiranhaMessage
    {
        public ChangeAvatarNameMessage(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Id = 10212;
        }

        public string Name { get; set; }

        private static readonly string[] bannedWords;

        static ChangeAvatarNameMessage()
        {
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string filterPath = Path.Combine(currentDirectory, "filter.json");

                if (File.Exists(filterPath))
                {
                    bannedWords = File.ReadAllLines(filterPath);
                }
                else
                {
                    bannedWords = Array.Empty<string>();
                }
            }
            catch
            {
                bannedWords = Array.Empty<string>();
            }
        }

        // Filter
        private string FilterMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return message;

            foreach (var word in bannedWords)
            {
                var replacement = new string('*', word.Length);
                message = Regex.Replace(
                    message,
                    Regex.Escape(word),
                    replacement,
                    RegexOptions.IgnoreCase
                );
            }
            return message;
        }

        public override void Decode()
        {
            Name = Reader.ReadScString();
        }

        public override async void Process()
        {
            if (string.IsNullOrEmpty(Name)) return;
            if (Name.Length < 2 || Name.Length > 15) return;

            var filteredName = FilterMessage(Name);

            var home = Device.Player.Home;
            if (home.NameSet >= 2) return;

            home.Name = filteredName;

            var info = Device.Player.Home.AllianceInfo;

            if (info.HasAlliance)
            {
                var alliance = await Resources.Alliances.GetAllianceAsync(info.Id);

                alliance.GetMember(home.Id).Name = filteredName;

                alliance.Save();
            }

            await new AvailableServerCommand(Device)
            {
                Command = new LogicChangeNameCommand(Device)
                {
                    NameSet = home.NameSet
                }
            }.SendAsync();

            home.NameSet++;

            Device.Player.Save();
        }
    }
}
using System;
using ClashRoyale.Logic;
using ClashRoyale.Logic.Home;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;
using ClashRoyale.Database;
using DotNetty.Buffers;
using System.Threading.Tasks;
using ClashRoyale.Logic.Clan;

namespace ClashRoyale.Protocol
{
    internal class LogicSendAllianceMailCommand : LogicCommand
    {
        private string title, desc;

        public LogicSendAllianceMailCommand(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            // Console.WriteLine("[DEBUG] LogicSendAllianceMailCommand called"); //debug
        }

        public override void Decode()
        {
            try
            {
                desc  = Reader.ReadScString(); // description
                title = Reader.ReadScString(); // title
                // Console.WriteLine($"[DEBUG] Title: {title}");
                // Console.WriteLine($"[DEBUG] Desc : {desc}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Exception in Decode: " + ex);
            }
        }

        public override void Process()
        {
            _ = ProcessAsync();
        }

        private async Task ProcessAsync()
        {
            var player = Device.Player;
            var allianceInfo = player.Home?.AllianceInfo;
            if (allianceInfo == null)
            {
                await new ServerErrorMessage(Device)
                {
                    Message = "You are not in a clan."
                }.SendAsync();
                return;
            }

            TimeSpan clanMailCooldown = TimeSpan.FromHours(12);
            if (player.Home.LastClanMailSent > DateTime.UtcNow - clanMailCooldown)
            {
                var nextAvailable = player.Home.LastClanMailSent + clanMailCooldown;
                var wait = nextAvailable - DateTime.UtcNow;
                await new ServerErrorMessage(Device)
                {
                    Message = $"You must wait {wait.Hours}h {wait.Minutes}m before sending another clan mail."
                }.SendAsync();
                return;
            }
            player.Home.LastClanMailSent = DateTime.UtcNow;
            player.Save();

            var alliance = await Resources.Alliances.GetAllianceAsync(allianceInfo.Id);
            if (alliance == null)
            {
                await new ServerErrorMessage(Device)
                {
                    Message = "Clan not found."
                }.SendAsync();
                return;
            }

            int sentCount = 0;
            foreach (var member in alliance.Members)
            {
                var memberPlayer = await member.GetPlayerAsync(true);
                if (memberPlayer == null)
                    memberPlayer = await member.GetPlayerAsync(false);

                if (memberPlayer != null)
                {
                    memberPlayer.Home.AddInboxMessage(new InboxMessage
                    {
                        Title = title,
                        Description = desc,
                        Timestamp = DateTime.UtcNow,
                        SenderId = player.Home.Id,
                        SenderName = player.Home.Name,
                        IsClanMail = true,
                        IconUrl = null,
                        ButtonText = null,
                        ButtonUrl = null
                    });

                    memberPlayer.Save();
                    sentCount++;
                }
            }
        }
    }
}
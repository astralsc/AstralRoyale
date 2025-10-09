using System;
using ClashRoyale.Logic;
using ClashRoyale.Logic.Home;
using ClashRoyale.Utilities.Netty;
using System.Collections.Generic;

namespace ClashRoyale.Protocol.Messages.Server
{
    public class InboxListMessage : PiranhaMessage
    {
        public InboxListMessage(Device device) : base(device)
        {
            Id = 24445;
        }

        public override void Encode()
        {
            var inbox = Device.Player.Home.InboxMessages ?? new List<InboxMessage>();
            Writer.WriteInt(inbox.Count);
            foreach (var msg in inbox)
            {
                Writer.WriteScString(msg.IconUrl ?? "");
                Writer.WriteScString(msg.Title ?? "");
                Writer.WriteScString(msg.Description ?? "");
                Writer.WriteScString(null);
                Writer.WriteScString(null);
                //Writer.WriteScString(msg.ButtonText ?? "");
                //Writer.WriteScString(msg.ButtonUrl ?? "");
                Writer.WriteScString(msg.SenderName ?? "");
                Writer.WriteScString(msg.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                Writer.WriteScString(msg.IsClanMail ? "clan" : "system");
            }
        }
    }
}

// Original before Clan Mail Update:

/*using ClashRoyale.Logic;
using ClashRoyale.Utilities.Netty;

namespace ClashRoyale.Protocol.Messages.Server
{
    public class InboxListMessage : PiranhaMessage
    {
        public InboxListMessage(Device device) : base(device)
        {
            Id = 24445;
        }

        public override void Encode()
        {
            Writer.WriteInt(3); // How many inbox messages that can show

            Writer.WriteScString("https://images.cults3d.com/SRTpmVRQDqzMXqOmQl8wPTUyEb8=/516x516/filters:no_upscale()/https://fbi.cults3d.com/uploaders/25298172/illustration-file/ccb1ffe5-b21c-471d-a22c-dc3fdda9f55e/465063822_27522622324049372_3765926730117400355_n-removebg-preview.png"); // Logo
            Writer.WriteScString("<c4>AstralRoyale</c>"); // Title
            Writer.WriteScString("A RetroRoyale fork created by @astralsc on GitHub!"); // Description
            Writer.WriteScString("Visit Repository"); // Button
            Writer.WriteScString("https://github.com/fdz6/AstralRoyale"); // URL

            // Seperation between inbox messages
            Writer.WriteScString(""); // Unk
            Writer.WriteScString(""); // Unk
            Writer.WriteScString(""); // Unk
            ////////////////////////////////////

            Writer.WriteScString("https://www.nicepng.com/png/full/141-1410330_clash-royale-red-king-logo-red-king-clash.png"); // Logo
            Writer.WriteScString("<c3>AstralRoyale Discord</c>"); // Title
            Writer.WriteScString("Join the official AstralRoyale discord server for the latest news!"); // Description
            Writer.WriteScString("Join Server"); // Button
            Writer.WriteScString("https://www.discord.gg/mUredE6CTU"); // URL

            // Seperation between inbox messages
            Writer.WriteScString(""); // Unk
            Writer.WriteScString(""); // Unk
            Writer.WriteScString(""); // Unk
            ////////////////////////////////////

            Writer.WriteScString("https://www.pngkey.com/png/full/435-4357630_renders-de-clash-royale-png.png"); // Logo
            Writer.WriteScString("<c2>AstralSC YouTube</c>"); // Title
            Writer.WriteScString("Subscribe to AstralSC for videos or posts!"); // Description
            Writer.WriteScString("Subscribe"); // Button
            Writer.WriteScString("https://www.youtube.com/@astral_sc"); // URL
        }
    }
}*/
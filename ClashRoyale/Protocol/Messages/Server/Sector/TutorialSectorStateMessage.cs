using System;
using System.IO;
using System.Linq;
using ClashRoyale.Logic;
using ClashRoyale.Logic.Battle;
using ClashRoyale.Utilities.Netty;
using ClashRoyale.Utilities.Utils;
using ClashRoyale.Protocol.Messages.Server;

namespace ClashRoyale.Protocol.Messages.Server
{
    public class TutorialSectorStateMessage : PiranhaMessage
    {
        public TutorialSectorStateMessage(Device device) : base(device)
        {
            Id = 21903;
            device.CurrentState = Device.State.Battle;
            MatchmakeInfoMessage.cant_battle_npc = true;
        }

        public override void Encode()
        {
            // Tutorial NPC
            Writer.WriteHex("0130020000789C9590314EC4301045FF4C3CD62E10A838C02AA20169450337E21620A70C08E5045BA44881B448BBDA2A250DD7A0E0081C00318E1D07509A7C4BA3D19F791E7BB072C750319353A117E18F1643F2DB27BED97D6C8FC6300592504B53B0E2D73A1089EC657C4FCD5F0F80B505C583C2F72B241012964C8C08F2F69336750638BCDD83E30DF9EB6D7BE7BDD2A481EA8D8DC9543C7526FAE599BA75340709A15F4BBF9C80BBA84054EFC3E509D1E1C6316701927FD03C75E7D82F622CCFD05C0C8576DCEDDC34833EF95B7FC338C513E191E03F2C96D9B05683AC224BE044D3EA50361697B8FA010922443E");
        }
    }
}
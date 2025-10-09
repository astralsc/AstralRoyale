using System;
using System.Threading.Tasks;
using ClashRoyale.Logic;
using ClashRoyale.Utilities.Netty;

namespace ClashRoyale.Protocol.Messages.Server
{
    public class MatchmakeInfoMessage : PiranhaMessage
    {
        public int SecondsUntilBotComes = 10;

        public MatchmakeInfoMessage(Device device) : base(device)
        {
            Id = 24107;
        }

        public int EstimatedDuration { get; set; }

        public static bool cant_battle_npc = false;

        public override void Encode()
        {
            Writer.WriteInt(EstimatedDuration);

/*            if (!cant_battle_npc)
            {
                cant_battle_npc = true;
                _ = InitializeAsync();*/
            }
        }


/*        public async Task InitializeAsync()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(SecondsUntilBotComes));
                await new NpcSectorStateMessage(Device).SendAsync();
                Resources.Battles.Cancel(Device.Player);
            }
            catch (TaskCanceledException)
            {
            }
        }
    }*/
}
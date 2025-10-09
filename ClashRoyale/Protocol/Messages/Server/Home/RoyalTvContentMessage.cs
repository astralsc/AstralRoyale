using ClashRoyale.Database;
using ClashRoyale.Logic;
using ClashRoyale.Utilities.Netty;

namespace ClashRoyale.Protocol.Messages.Server
{
    public class RoyalTvContentMessage : PiranhaMessage
    {
        public RoyalTvContentMessage(Device device) : base(device)
        {
            Id = 24405;
        }

        public int ClassId { set; get; }
        public int InstanceId { set; get; }

        public override void Encode()
        {
            Writer.WriteVInt(0);

            Writer.WriteScString("{\"player0\":{\"acc_hi\":0,\"acc_lo\":1,\"name\":\"Test 1\",\"alliance\":\"Test\",\"stars\":1,\"score\":0,\"score_p\":30,\"alli_hi\":0,\"alli_lo\":1,\"home_hi\":0,\"home_lo\":1,\"badge\":16000078,\"spells\":[{\"d\":26000006},{\"d\":26000020},{\"d\":28000004},{\"d\":26000018,\"l\":1},{\"d\":26000011},{\"d\":26000003,\"l\":2},{\"d\":26000014,\"l\":1},{\"d\":26000012}]},\"player1\":{\"acc_hi\":0,\"acc_lo\":2,\"name\":\"Test 2\",\"alliance\":\"Test\",\"stars\":3,\"score\":30,\"score_p\":0,\"alli_hi\":0,\"alli_lo\":1,\"home_hi\":0,\"home_lo\":2,\"badge\":16000078,\"spells\":[{\"d\":26000000,\"l\":1},{\"d\":26000007},{\"d\":26000013},{\"d\":26000018},{\"d\":28000000},{\"d\":26000003},{\"d\":26000002},{\"d\":26000015}]},\"player2\":{\"acc_hi\":0,\"acc_lo\":0,\"alli_hi\":0,\"alli_lo\":0,\"home_hi\":0,\"home_lo\":0},\"player3\":{\"acc_hi\":0,\"acc_lo\":0,\"alli_hi\":0,\"alli_lo\":0,\"home_hi\":0,\"home_lo\":0},\"arena\":54000002,\"replayV\":64,\"challenge\":false,\"tournament\":false,\"friendly_challenge\":false,\"survival\":false,\"game_config\":{\"gmt\":1,\"plt\":1,\"gamemode\":72000006,\"t1s\":0,\"t2s\":0}}");

            Writer.WriteVInt(0);

            // Replay Version
            Writer.WriteVInt(3);
            Writer.WriteVInt(377); 
            Writer.WriteVInt(8); 

            Writer.WriteVInt(99999); // Views

            Writer.WriteVInt(0);
            Writer.WriteVInt(1);
            Writer.WriteVInt(3600); // Age

            Writer.WriteVInt(0); // ReplayShardId?

            Writer.WriteVInt(1);
            Writer.WriteVInt(26);

            Writer.WriteLong(1783956694); // ReplayId

            Writer.WriteVInt(ClassId);
            Writer.WriteVInt(InstanceId);
        }
    }
}
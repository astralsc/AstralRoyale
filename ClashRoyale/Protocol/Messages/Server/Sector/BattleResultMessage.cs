using ClashRoyale.Logic;
using ClashRoyale.Utilities.Netty;
using ClashRoyale.Files.CsvLogic;

namespace ClashRoyale.Protocol.Messages.Server
{
    public class BattleResultMessage : PiranhaMessage
    {
        public BattleResultMessage(Device device) : base(device)
        {
            Id = 20225;
        }

        public int TrophyReward { get; set; }
        public int OpponentTrophyReward { get; set; }
        public int BattleResultType { get; set; }
        public int OwnCrowns { get; set; }
        public int OpponentCrowns { get; set; }
        //public TreasureChests AwardedChest { get; set; }


        public override void Encode()
        {
            Writer.WriteVInt(BattleResultType);
            Writer.WriteVInt(TrophyReward);

            Writer.WriteVInt(0);
            Writer.WriteVInt(OpponentTrophyReward);

            Writer.WriteVInt(0);
            Writer.WriteVInt(63);

            Writer.WriteVInt(0);
            Writer.WriteVInt(0);

            Writer.WriteVInt(OwnCrowns);
            Writer.WriteVInt(OpponentCrowns);

            // --- CHEST REWARD ---
            /*if (AwardedChest != null)
            {*/
                Writer.WriteVInt(219); // TreasureChests ClassID
                Writer.WriteVInt(19); // The specific InstanceID of the chest AwardedChest.GetInstanceId()
            /*}
            else
            {
                // No chest awarded
                Writer.WriteVInt(0);
                Writer.WriteVInt(0);
            }*/

            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(4);
            Writer.WriteVInt(47);
            Writer.WriteVInt(1260);
            Writer.WriteVInt(1293);
            Writer.WriteVInt(11);
            Writer.WriteVInt(1260);

            /*if (AwardedChest != null)
            {*/
                Writer.WriteVInt(219); // TreasureChestData ClassID
                Writer.WriteVInt(19);
            /*}
            else
            {
                Writer.WriteVInt(0);
                Writer.WriteVInt(0);
            }*/

            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
            Writer.WriteVInt(0);
        }
    }
}
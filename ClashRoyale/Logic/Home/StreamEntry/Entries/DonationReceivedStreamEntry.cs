using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Logic.Home.StreamEntry.Entries
{
    public class DonationReceivedStreamEntry : AvatarStreamEntry
    {
        public int CardType { get; set; }
        public int CardInstance { get; set; }
        public int Amount { get; set; }
        public string DonorName { get; set; }

        public DonationReceivedStreamEntry(int cardType, int cardInstance, int amount, string donorName)
        {
            StreamEntryType = 7;
            CardType = cardType;
            CardInstance = cardInstance;
            Amount = amount;
            DonorName = donorName;
        }

        public override void Encode(IByteBuffer packet)
        {
            base.Encode(packet);
            packet.WriteVInt(Amount);
            packet.WriteVInt(CardType);
            packet.WriteVInt(CardInstance);
            packet.WriteScString(DonorName ?? string.Empty);
        }
    }
}
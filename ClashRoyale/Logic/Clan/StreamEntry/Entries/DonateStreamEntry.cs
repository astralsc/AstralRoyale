using System;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;
using Newtonsoft.Json;


namespace ClashRoyale.Logic.Clan.StreamEntry.Entries
{
    public class DonateStreamEntry : AllianceStreamEntry
    {
        public DonateStreamEntry()
        {
            StreamEntryType = 1;
        }

        [JsonProperty("msg")] public string Message { get; set; }
        [JsonProperty("totalCapacity")] public int TotalCapacity { get; set; }
        [JsonProperty("usedCapacity")] public int UsedCapacity { get; set; }
        [JsonProperty("cardType")] public int CardType { get; set; }
        [JsonProperty("cardInstance")] public int CardInstance { get; set; }

        public override void Encode(IByteBuffer packet)
        {
            Console.WriteLine($"[Active donation in clan] SenderId={SenderId}, CardType={CardType}, CardInstance={CardInstance}, TotalCapacity={TotalCapacity}, UsedCapacity={UsedCapacity}, Message={Message}");

            if (TotalCapacity < 0 || UsedCapacity < 0)
            {
                throw new Exception($"Invalid capacities! TotalCapacity={TotalCapacity}, UsedCapacity={UsedCapacity}");
            }

            base.Encode(packet);

            packet.WriteVInt(StreamEntryType);             // 1 = donation request??
            packet.WriteLong(SenderId);                    // Who sent the request
            packet.WriteVInt(CardType);                    // Card type (e.g., 26 = troop, 28 = spell)
            packet.WriteVInt(CardInstance);                // Card ID
            packet.WriteVInt(TotalCapacity);               // Max cards that can be donated
            packet.WriteVInt(UsedCapacity);                // Donated so far??
            packet.WriteVInt(0);                           // Number of donations??
            packet.WriteScString(Message ?? string.Empty); // Request message
        }
    }
}
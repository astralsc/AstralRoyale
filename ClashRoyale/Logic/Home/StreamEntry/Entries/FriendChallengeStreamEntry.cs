using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;
using Newtonsoft.Json;

namespace ClashRoyale.Logic.Home.StreamEntry.Entries
{
    public class FriendChallengeStreamEntry : AvatarStreamEntry
    {
        public FriendChallengeStreamEntry()
        {
            StreamEntryType = 5;
        }

        [JsonProperty("msg")] public string Message { get; set; }
        [JsonProperty("sender_score")] public int SenderScore { get; set; }
        [JsonProperty("arena")] public int Arena { get; set; }
        [JsonProperty("closed")] public bool Closed { get; set; }
        [JsonProperty("active")] public bool Active { get; set; }
        [JsonProperty("target_name")] public string TargetName { get; set; }

        [JsonIgnore] public int Spectators { get; set; }

        public override void Encode(IByteBuffer packet)
        {
            base.Encode(packet);
        }
    }
}
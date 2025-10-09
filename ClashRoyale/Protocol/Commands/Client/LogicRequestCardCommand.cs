using System;
using System.Collections.Generic;
using System.Linq;
using ClashRoyale.Logic;
using ClashRoyale.Logic.Clan;
using ClashRoyale.Logic.Clan.StreamEntry.Entries;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol.Commands.Client
{
    public class LogicRequestCardCommand : LogicCommand
    {
        public int CardId;
        public int Amount;
        public int CardType;
        public int CardInstance;

        private static readonly TimeSpan RequestCooldown = TimeSpan.FromHours(7);

        public class CardInfo
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Rarity { get; set; }
        }

        public static readonly Dictionary<int, CardInfo> CardDatabase = new Dictionary<int, CardInfo>
        {
            [26000000] = new CardInfo { ID = 26000000, Name = "Knight", Rarity = "Common" },
            [26000001] = new CardInfo { ID = 26000001, Name = "Archers", Rarity = "Common" },
            [26000002] = new CardInfo { ID = 26000002, Name = "Goblins", Rarity = "Common" },
            [26000005] = new CardInfo { ID = 26000005, Name = "Minions", Rarity = "Common" },
            [26000008] = new CardInfo { ID = 26000008, Name = "Barbarians", Rarity = "Common" },
            [26000010] = new CardInfo { ID = 26000010, Name = "Skeletons", Rarity = "Common" },
            [26000013] = new CardInfo { ID = 26000013, Name = "Bomber", Rarity = "Common" },
            [26000019] = new CardInfo { ID = 26000019, Name = "Spear Goblins", Rarity = "Common" },
            [26000022] = new CardInfo { ID = 26000022, Name = "Minion Horde", Rarity = "Common" },
            [26000024] = new CardInfo { ID = 26000024, Name = "Royal Giant", Rarity = "Common" },
            [26000030] = new CardInfo { ID = 26000030, Name = "Ice Spirit", Rarity = "Common" },
            [26000031] = new CardInfo { ID = 26000031, Name = "Fire Spirits", Rarity = "Common" },
            [26000041] = new CardInfo { ID = 26000041, Name = "Goblin Gang", Rarity = "Common" },
            [26000043] = new CardInfo { ID = 26000043, Name = "Elite Barbarians", Rarity = "Common" },
            [26000049] = new CardInfo { ID = 26000049, Name = "Bats", Rarity = "Common" },
            [26000056] = new CardInfo { ID = 26000056, Name = "Skeleton Barrel", Rarity = "Common" },
            [28000001] = new CardInfo { ID = 28000001, Name = "Arrows", Rarity = "Common" },
            [28000008] = new CardInfo { ID = 28000008, Name = "Zap", Rarity = "Common" },
            [26000003] = new CardInfo { ID = 26000003, Name = "Giant", Rarity = "Rare" },
            [26000011] = new CardInfo { ID = 26000011, Name = "Valkyrie", Rarity = "Rare" },
            [26000014] = new CardInfo { ID = 26000014, Name = "Musketeer", Rarity = "Rare" },
            [26000017] = new CardInfo { ID = 26000017, Name = "Wizard", Rarity = "Rare" },
            [26000018] = new CardInfo { ID = 26000018, Name = "Mini Pekka", Rarity = "Rare" },
            [26000021] = new CardInfo { ID = 26000021, Name = "Hog Rider", Rarity = "Rare" },
            [26000028] = new CardInfo { ID = 26000028, Name = "Three Musketeers", Rarity = "Rare" },
            [26000036] = new CardInfo { ID = 26000036, Name = "Battle Ram", Rarity = "Rare" },
            [26000038] = new CardInfo { ID = 26000038, Name = "Ice Golem", Rarity = "Rare" },
            [26000039] = new CardInfo { ID = 26000039, Name = "Mega Minion", Rarity = "Rare" },
            [26000040] = new CardInfo { ID = 26000040, Name = "Dart Goblin", Rarity = "Rare" },
            [26000057] = new CardInfo { ID = 26000057, Name = "Flying Machine", Rarity = "Rare" },
            [28000000] = new CardInfo { ID = 28000000, Name = "Fireball", Rarity = "Rare" },
            [28000003] = new CardInfo { ID = 28000003, Name = "Rocket", Rarity = "Rare" },
            [28000016] = new CardInfo { ID = 28000016, Name = "Heal Potion", Rarity = "Rare" },
            [26000004] = new CardInfo { ID = 26000004, Name = "Pekka", Rarity = "Epic" },
            [26000006] = new CardInfo { ID = 26000006, Name = "Balloon", Rarity = "Epic" },
            [26000007] = new CardInfo { ID = 26000007, Name = "Witch", Rarity = "Epic" },
            [26000009] = new CardInfo { ID = 26000009, Name = "Golem", Rarity = "Epic" },
            [26000012] = new CardInfo { ID = 26000012, Name = "Skeleton Army", Rarity = "Epic" },
            [26000015] = new CardInfo { ID = 26000015, Name = "Baby Dragon", Rarity = "Epic" },
            [26000016] = new CardInfo { ID = 26000016, Name = "Prince", Rarity = "Epic" },
            [26000020] = new CardInfo { ID = 26000020, Name = "Giant Skeleton", Rarity = "Epic" },
            [26000025] = new CardInfo { ID = 26000025, Name = "Guards", Rarity = "Epic" },
            [26000034] = new CardInfo { ID = 26000034, Name = "Bowler", Rarity = "Epic" },
            [26000045] = new CardInfo { ID = 26000045, Name = "Executioner", Rarity = "Epic" },
            [26000054] = new CardInfo { ID = 26000054, Name = "Cannon Cart", Rarity = "Epic" },
            [28000002] = new CardInfo { ID = 28000002, Name = "Rage", Rarity = "Epic" },
            [28000004] = new CardInfo { ID = 28000004, Name = "Goblin Barrel", Rarity = "Epic" },
            [28000005] = new CardInfo { ID = 28000005, Name = "Freeze", Rarity = "Epic" },
            [28000006] = new CardInfo { ID = 28000006, Name = "Mirror", Rarity = "Epic" },
            [28000007] = new CardInfo { ID = 28000007, Name = "Lightning", Rarity = "Epic" },
            [28000009] = new CardInfo { ID = 28000009, Name = "Poison", Rarity = "Epic" },
            [28000012] = new CardInfo { ID = 28000012, Name = "Tornado", Rarity = "Epic" },
            [28000013] = new CardInfo { ID = 28000013, Name = "Clone", Rarity = "Epic" },
            [26000023] = new CardInfo { ID = 26000023, Name = "Ice Wizard", Rarity = "Legendary" },
            [26000026] = new CardInfo { ID = 26000026, Name = "Princess", Rarity = "Legendary" },
            [26000029] = new CardInfo { ID = 26000029, Name = "Lava Hound", Rarity = "Legendary" },
            [26000032] = new CardInfo { ID = 26000032, Name = "Miner", Rarity = "Legendary" },
            [26000033] = new CardInfo { ID = 26000033, Name = "Sparky", Rarity = "Legendary" },
            [26000035] = new CardInfo { ID = 26000035, Name = "Lumberjack", Rarity = "Legendary" },
            [26000037] = new CardInfo { ID = 26000037, Name = "Inferno Dragon", Rarity = "Legendary" },
            [26000042] = new CardInfo { ID = 26000042, Name = "Electro Wizard", Rarity = "Legendary" },
            [26000046] = new CardInfo { ID = 26000046, Name = "Bandit", Rarity = "Legendary" },
            [26000048] = new CardInfo { ID = 26000048, Name = "Night Witch", Rarity = "Legendary" },
            [26000055] = new CardInfo { ID = 26000055, Name = "Mega Knight", Rarity = "Legendary" },
            [28000010] = new CardInfo { ID = 28000010, Name = "Graveyard", Rarity = "Legendary" },
            [28000011] = new CardInfo { ID = 28000011, Name = "Log", Rarity = "Legendary" },
        };

        public LogicRequestCardCommand(Device device, IByteBuffer reader) : base(device, reader) { }

        public override void Decode()
        {
            int startIndex = Reader.ReaderIndex;
            var vints = new List<int>();
            int limit = 6;
            for (int i = 0; i < limit; i++)
            {
                if (Reader.IsReadable())
                    vints.Add(Reader.ReadVInt());
                else
                    break;
            }
            int endIndex = Reader.ReaderIndex;
            int length = endIndex - startIndex;
            byte[] rawBytes = new byte[length];
            Reader.SetReaderIndex(startIndex);
            Reader.ReadBytes(rawBytes, 0, length);
            Reader.SetReaderIndex(endIndex);

            CardType = vints.Count > 4 ? vints[4] : -1;
            CardInstance = vints.Count > 5 ? vints[5] : -1;
            CardId = (CardType > 0 && CardInstance >= 0) ? CardType * 1_000_000 + CardInstance : -1;
            Amount = 1;

            Console.WriteLine($"[Donations] VInts: {string.Join(", ", vints)} (Type={CardType}, Instance={CardInstance}, CardId={CardId}, Amount={Amount})");
            Console.Write("[Donations] Raw HEX: ");
            for (int i = 0; i < rawBytes.Length; i++)
                Console.Write($"{rawBytes[i]:X2} ");
            Console.WriteLine();
        }

        public override void Process()
        {
            Console.WriteLine("[Donations] Process called.");
            var player = Device.Player;
            if (player == null || !player.Home.AllianceInfo.HasAlliance)
            {
                Console.WriteLine("[Donations] Player not in clan or player is null.");
                return;
            }

            // Check cooldown
            var now = DateTime.UtcNow;
            if (player.Home.NextCardRequestTime > now)
            {
                var seconds = (int)(player.Home.NextCardRequestTime - now).TotalSeconds;
                var sendErrorTask = new Protocol.Messages.Server.ServerErrorMessage(Device)
                {
                    Message = $"You must wait {seconds / 60}m {seconds % 60}s before requesting again!"
                }.SendAsync();
                sendErrorTask.Wait();
                Console.WriteLine($"[Donations] Card request cooldown not expired for player {player.Home.Name}.");
                return;
            }

            var allianceTask = Resources.Alliances.GetAllianceAsync(player.Home.AllianceInfo.Id);
            allianceTask.Wait();
            var alliance = allianceTask.Result;
            if (alliance == null)
            {
                Console.WriteLine("[Donations] Clan not found.");
                return;
            }

            // Remove previous donation requests by this player
            lock (alliance.Stream)
            {
                var oldEntries = alliance.Stream
                    .OfType<DonateStreamEntry>()
                    .Where(e => e.SenderId == player.Home.Id)
                    .ToList();

                foreach (var entry in oldEntries)
                {
                    alliance.RemoveEntry(entry);
                }
            }

            CardInfo cardData = null;
            CardDatabase.TryGetValue(CardId, out cardData);
            string cardName = cardData != null ? cardData.Name : $"ID: {CardId}";
            string rarity = cardData != null ? cardData.Rarity : "Common";
            int arena = player.Home.Arena.CurrentArena;
            int totalCapacity = GetDonationCapacity(rarity, arena);

            var donateEntry = new DonateStreamEntry
            {
                Message = $"Requested Card: {cardName}",
                TotalCapacity = totalCapacity,
                UsedCapacity = 0,
                CardType = CardType,
                CardInstance = CardInstance
            };
            donateEntry.SetSender(player);
            alliance.AddEntry(donateEntry);

            // Set cooldown (7 hours)
            player.Home.NextCardRequestTime = now + RequestCooldown;

            var sendOkTask = new Protocol.Messages.Server.ServerErrorMessage(Device)
            {
                Message = "Donation submitted"
            }.SendAsync();
            sendOkTask.Wait();

            alliance.Save();
            player.Save();

            Console.WriteLine($"[Donations] Donation submitted for {player.Home.Name}.");
        }

        private int GetDonationCapacity(string rarity, int arena)
        {
            if (rarity == null) return 1;
            if (rarity.Equals("Common", StringComparison.OrdinalIgnoreCase))
            {
                if (arena == 1) return 10;
                if (arena == 2 || arena == 3) return 10;
                if (arena >= 4 && arena <= 6) return 20;
                if (arena >= 7 && arena <= 9) return 30;
                if (arena >= 10) return 40;
            }
            if (rarity.Equals("Rare", StringComparison.OrdinalIgnoreCase))
            {
                if (arena == 1) return 1;
                if (arena == 2 || arena == 3) return 1;
                if (arena >= 4 && arena <= 6) return 2;
                if (arena >= 7 && arena <= 9) return 3;
                if (arena >= 10) return 4;
            }
            // For Epics/Legendaries: not requestable in normal clan requests, but fallback to 1.
            return 1;
        }
    }
}
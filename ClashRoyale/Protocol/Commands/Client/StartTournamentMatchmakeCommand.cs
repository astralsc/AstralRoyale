using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClashRoyale.Logic;
using ClashRoyale.Logic.Battle;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol
{
    internal class StartTournamentMatchmakeCommand : LogicCommand
    {
        private readonly Random _random = new Random();

        public StartTournamentMatchmakeCommand(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Process();
        }

        public bool IsTournament { get; set; }

        public override void Decode()
        {
            base.Decode();

            Reader.ReadVInt();
            Reader.ReadVInt();

            IsTournament = Reader.ReadBoolean();
        }

        private static readonly Dictionary<int, int> ArenaValueTable = GenerateRandomArenaTable();

        private static Dictionary<int, int> GenerateRandomArenaTable()
        {
            var numbers = Enumerable.Range(1, 11).ToList();
            var random = new Random();
            var shuffled = numbers.OrderBy(_ => random.Next()).ToList();

            var table = new Dictionary<int, int>();
            for (int i = 1; i <= 11; i++)
            {
                table[i] = shuffled[i - 1];
            }

            return table;
        }

        public override async void Process()
        {
            IsTournament = true;
            await new MatchmakeInfoMessage(Device)
            {
                EstimatedDuration = _random.Next(101, 601)
            }.SendAsync();

/*            var enemy = Resources.TournamentBattles.Dequeue;
            if (enemy != null)
            {
                int randomKey = ArenaValueTable.Keys.ElementAt(_random.Next(ArenaValueTable.Count));

                int randomizedArena = ArenaValueTable[randomKey];

                var battle = new LogicBattle(false, randomizedArena + 1, true)
                {
                    Device.Player, enemy
                };

                Resources.TournamentBattles.Add(battle);

                Device.Player.Battle = battle;
                enemy.Battle = battle;

                battle.Start();
            }
            else
            {
                Resources.TournamentBattles.Enqueue(Device.Player);
            }*/
        }
    }
}
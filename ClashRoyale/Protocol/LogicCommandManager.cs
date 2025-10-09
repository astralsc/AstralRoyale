using System;
using System.Diagnostics;
using System.Collections.Generic;
using ClashRoyale.Protocol.Commands.Client;
using ClashRoyale.Protocol.Commands.Server;

namespace ClashRoyale.Protocol
{
    public class LogicCommandManager
    {
        public static Dictionary<int, Type> Commands;

        static LogicCommandManager()
        {
            Commands = new Dictionary<int, Type>
            {
                {1, typeof(DoSpellCommand)},
                {500, typeof(LogicSwapSpellsCommand)},
                {501, typeof(LogicSelectDeckCommand)},
                //{502, typeof(LogicStartChestUnlockCommand)},  // Start unlocking chest
                //{503, typeof(LogicCollectChestCommand)}, // Unlock Chest
                {504, typeof(LogicFuseSpellsCommand)},
                //{505, typeof(LogicCollectChestCommand)}, // Unlock Chest (With Gems)
                {507, typeof(LogicBuyResourcePackCommand)},
                {508, typeof(LogicXPLevelUpCommand)}, // TODO
                {509, typeof(LogicCollectFreeChestCommand)},
                {511, typeof(LogicCollectCrownChestCommand)},
                {512, typeof(LogicRequestCardCommand)},
                {513, typeof(LogicFreeWorkerCommand)},
                {514, typeof(LogicKickAllianceMemberCommand)},
                {516, typeof(LogicBuyChestCommand)},
                {517, typeof(LogicBuyResourcesCommand)},
                {518, typeof(LogicBuySpellCommand)},
                //{520, typeof(LogicShopSeenCommand)},
                {521, typeof(LogicSendAllianceMailCommand)}, // TODO
                {522, typeof(LogicChallengeCommand)},
                {523, typeof(ClaimAchievementsCommand)}, // TODO
                {524, typeof(LogicRequestCardCommand)},
                {525, typeof(StartMatchmakeCommand)},
                {526, typeof(LogicChestNextCardCommand)},
                {529, typeof(LogicCopyDeckCommand)},
                {531, typeof(LogicCreateTournamentCommand)},
                {536, typeof(LogicTvReplaySeenCommand)},
                {537, typeof(EnterTournamentCommand)}, // TODO
                {539, typeof(StartTournamentMatchmakeCommand)}, // TODO
                {551, typeof(LogicFoundDonationCommand)}, // TODO
                {555, typeof(LogicBuyGemsCommand)}, // Rewards free gems instead of buying with actual money
                {557, typeof(LogicFriendChallengeCommand)} // TODO
            };
        }
    }
}
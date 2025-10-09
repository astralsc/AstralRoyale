using System;
using System.IO;
using System.Linq;
using ClashRoyale;
using ClashRoyale.Database;
using ClashRoyale.Logic;
using ClashRoyale.Logic.Clan.StreamEntry.Entries;
using ClashRoyale.Logic.Home.Decks;
using ClashRoyale.Logic.Battle;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol.Messages.Client.Alliance
{
    public class ChatToAllianceStreamMessage : PiranhaMessage
    {
        public ChatToAllianceStreamMessage(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Id = 14315;
        }

        public string Message { get; set; }

        public override void Decode()
        {
            Message = Reader.ReadScString();
        }

        public void SendMessage(string message, Logic.Clan.Alliance alliance)
        {
            var entry = new ChatStreamEntry
            {
                Message = message
            };

            entry.SetSender(Device.Player);

            alliance.AddEntry(entry);
        }

        public override async void Process()
        {
            var info = Device.Player.Home.AllianceInfo;
            if (!info.HasAlliance) return;

            var alliance = await Resources.Alliances.GetAllianceAsync(info.Id);
            if (alliance == null) return;

            if (Message.StartsWith('/'))
            {
                var cmd = Message.Split(' ');
                var cmdType = cmd[0];
                var cmdValue = 0;

                if (cmd.Length > 1)
                    if (Message.Split(' ')[1].Any(char.IsDigit))
                        int.TryParse(Message.Split(' ')[1], out cmdValue);

                switch (cmdType)
                {
                    case "/max":
                    {
                        var deck = Device.Player.Home.Deck;

                        foreach (var card in Cards.GetAllCards())
                        {
                            deck.Add(card);

                            for (var i = 0; i < 12; i++) deck.UpgradeCard(card.ClassId, card.InstanceId, true);
                        }

                        await new ServerErrorMessage(Device)
                        {
                            Message = "Added all cards with max level"
                        }.SendAsync();

                        break;
                    }

                    case "/unlock":
                    {
                        var deck = Device.Player.Home.Deck;

                        foreach (var card in Cards.GetAllCards()) deck.Add(card);

                        await new ServerErrorMessage(Device)
                        {
                            Message = "Added all cards"
                        }.SendAsync();

                        break;
                    }

                    case "/gold":
                    {
                        Device.Player.Home.Gold += cmdValue;
                        Device.Disconnect();
                        break;
                    }

                    case "/gems":
                    {
                        Device.Player.Home.Diamonds += cmdValue;
                        Device.Disconnect();
                        break;
                    }

                    case "/status":
                    {
                        var entry = new ChatStreamEntry
                        {
                            Message =
                                $"Server Status:\nBuild Version: 1.5 (for 1.9.2)\nFingerprint SHA:\n{Resources.Fingerprint.Sha}\nOnline Players: {Resources.Players.Count}\nTotal Players: {await PlayerDb.CountAsync()}\nTotal Clans: {await AllianceDb.CountAsync()}\n1v1 Battles: {Resources.Battles.Count}\n2v2 Battles: {Resources.DuoBattles.Count}\nTournament Battles: {Resources.TournamentBattles.Count}\nUptime: {DateTime.UtcNow.Subtract(Resources.StartTime).ToReadableString()}\nUsed RAM: {System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024) + " MB"}"
                        };

                        entry.SetSender(Device.Player);

                        alliance.AddEntry(entry);

                        break;
                    }

                    case "/help":
                    {
                        var entry = new ChatStreamEntry
                        {
                            Message = $"Commands:\n/help\n/status\n/max\n/unlock\n/gold x\n/gems x\n/free\n/adminhelp"
                        };

                        entry.SetSender(Device.Player);

                        alliance.AddEntry(entry);

                        break;
                    }

                    case "/adminhelp":
                    {
                        var entry = new ChatStreamEntry
                        {
                            Message = $"Admin Commands:\n/adminhelp\n/maintenance\n/admin\n/ban\n/unban\n/trophies x\n/settrophies"
                        };

                        entry.SetSender(Device.Player);

                        alliance.AddEntry(entry);

                        break;
                    }

                    case "/free":
                    {
                        Device.Player.Home.FreeChestTime = Device.Player.Home.FreeChestTime.Subtract(TimeSpan.FromMinutes(245));
                        Device.Disconnect();
                        break;
                    }

                    case "/replay":
                    {
                        if (ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin((int)Device.Player.Home.Id))
                        {
                            await new HomeBattleReplayDataMessage(Device).SendAsync();
                        }
                        else
                        {
                            var entry = new ChatStreamEntry
                            {
                                Message = "only admins can use the /replay command."
                            };

                            entry.SetSender(Device.Player);
                            alliance.AddEntry(entry);
                        }
                        break;
                    }

                    case "/maintenance":
                    {
                        if (ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin((int)Device.Player.Home.Id))
                        {
                            if (Resources.Configuration.Maintenance)
                            {
                                SendMessage($"[Info] Maintenance has been disabled.", alliance);
                                Logger.Log($"Maintenance has been disabled.", null);
                                Program.HandleMaintenanceThroughChat(cmdValue);
                            }
                            else
                            {
                                SendMessage($"[Info] Maintenance has been enabled.", alliance);
                                Logger.Log($"Maintenance has been enabled.", null);
                                Program.HandleMaintenanceThroughChat(cmdValue);
                            }
                        }
                        else
                        {
                            var entry = new ChatStreamEntry
                            {
                                Message = "only admins can use the /maintenance command."
                            };

                            entry.SetSender(Device.Player);
                            alliance.AddEntry(entry);
                        }
                        break;
                    }

                    case "/ban":
                    {
                        if (ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin((int)Device.Player.Home.Id))
                        {
                            var player = await PlayerDb.GetAsync(cmdValue);

                            if (player == null)
                            {
                                SendMessage($"player with ID {cmdValue} was not found.", alliance);
                                break;
                            }

                            Resources.Configuration.BannedIds.Add(player.Home.Id);
                            Resources.Configuration.Save();
                            Resources.Configuration.Initialize();
                            SendMessage($"The player with ID {cmdValue} has been banned.", alliance);
                            Logger.Log($"The player with ID {cmdValue} has been banned.", GetType());
                        }
                        else
                        {
                            SendMessage("only admins can use /ban command.", alliance);
                        }
                        break;
                    }

                    case "/unban":
                    {
                        if (ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin((int)Device.Player.Home.Id))
                        {
                            var player = await PlayerDb.GetAsync(cmdValue);

                            if (player == null)
                            {
                                SendMessage($"player with ID {cmdValue} was not found.", alliance);
                                break;
                            }

                            Resources.Configuration.BannedIds.Remove(player.Home.Id);
                            Resources.Configuration.Save();
                            Resources.Configuration.Initialize();
                            SendMessage($"The player with ID {cmdValue} has been unbanned.", alliance);
                            Logger.Log($"The player with ID {cmdValue} has been unbanned.", GetType());
                        }
                        else
                        {
                            SendMessage("only admins can use /unban command.", alliance);
                        }
                        break;
                    }

                    case "/admin":
                    {
                        if (ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin((int)Device.Player.Home.Id))
                        {
                            var player = await PlayerDb.GetAsync(cmdValue);

                            if (player == null)
                            {
                                SendMessage($"player with ID {cmdValue} was not found.", alliance);
                                Logger.Log($"player with ID {cmdValue} was not found.", null);
                                break;
                            }

                            Resources.Configuration.Admins.Add(player.Home.Id);
                            Resources.Configuration.Save();
                            Resources.Configuration.Initialize();
                            SendMessage($"player with ID {cmdValue} is now an admin.", alliance);
                            Logger.Log($"player with ID {cmdValue} is now an admin.", GetType());
                        }
                        else
                        {
                            SendMessage("only admins can use /admin command.", alliance);
                        }
                        break;
                    }

                    case "/trophies":
                    {
                        if (ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin((int)Device.Player.Home.Id))
                        {
                            if (cmdValue >= 0)
                                Device.Player.Home.Arena.AddTrophies(cmdValue);
                            else if (cmdValue < 0)
                                Device.Player.Home.Arena.RemoveTrophies(cmdValue);
                                Device.Disconnect();
                                break;
                        }
                        else
                        {
                            var entry = new ChatStreamEntry
                            {
                                Message = "only admins can use the /trophies command."
                            };

                            entry.SetSender(Device.Player);
                            alliance.AddEntry(entry);
                        }
                        break;
                    }

                    case "/settrophies":
                    {
                        if (ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin((int)Device.Player.Home.Id))
                        {
                            Device.Player.Home.Arena.SetTrophies(cmdValue);
                            Device.Disconnect();
                        }
                        else
                        {
                            var entry = new ChatStreamEntry
                            {
                                Message = "only admins can use the /settrophies command."
                            };

                            entry.SetSender(Device.Player);
                            alliance.AddEntry(entry);
                        }
                        break;
                    }

                    case "/tutorial": // only for admin testing (TODO)
                    {
                        if (ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin((int)Device.Player.Home.Id))
                        {
                            Id = 14104;
                            RequiredState = Device.State.Home;
                            await new TutorialSectorStateMessage(Device).SendAsync();
                        }
                        else
                        {
                            var entry = new ChatStreamEntry
                            {
                                Message = "only admins can use the /tutorial command."
                            };

                            entry.SetSender(Device.Player);
                            alliance.AddEntry(entry);
                        }
                        break;
                    }

                    default:
                    var error = new ChatStreamEntry
                    {
                        Message = "Command not found. Use /help for the list of commands."
                    };
                    error.SetSender(Device.Player);
                    alliance.AddEntry(error);
                    break;
                }
            }
else
{
    string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
    string filterPath = Path.Combine(currentDirectory, "filter.json");
    string[] bannedWords = File.ReadAllLines(filterPath);

    string FilterMessage(string Message)
    {
        foreach (var word in bannedWords)
        {
            var replacement = new string('*', word.Length);
            Message = System.Text.RegularExpressions.Regex.Replace(
                Message,
                System.Text.RegularExpressions.Regex.Escape(word),
                replacement,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
        }
        return Message;
    }

    var filteredMessage = FilterMessage(Message);

    var entry = new ChatStreamEntry
    {
        Message = filteredMessage
    };

    entry.SetSender(Device.Player);

        alliance.AddEntry(entry);
}
}
}
}
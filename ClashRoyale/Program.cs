using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using ClashRoyale;
using ClashRoyale.Core;
using ClashRoyale.Database;
using ClashRoyale.Database.Cache;
using ClashRoyale.Logic.Clan;
using ClashRoyale.Utilities.Utils;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities;
using ClashRoyale.WebAPI;

namespace ClashRoyale
{
    public static class Program
    {
        // Local Build Version
        private static readonly string LocalVersion = "1.9.2_4"; // Your Server Version

        private static bool _isRunning = true;
        public static DateTime MaintenanceEndTime = DateTime.UtcNow;

        static async Task Main()
        {
            Console.Clear();

            Console.Title = $"AstralRoyale: V{ClashRoyale.Core.Configuration.Version} (by: @astralsc on GitHub)";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string remoteUrl = "https://raw.githubusercontent.com/astralsc/AstralRoyale/refs/heads/master/project_update_check";
            string remotereasonUrl = "https://raw.githubusercontent.com/astralsc/AstralRoyale/refs/heads/master/project_update_reason_check";
            string ProjectPage = "https://github.com/astralsc/AstralRoyale/releases/tag/Server";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string latestVersion = await client.GetStringAsync(remoteUrl);
                    latestVersion = latestVersion.Trim();
                    
                    string latestVersion_changelog = await client.GetStringAsync(remotereasonUrl);
                    latestVersion_changelog = latestVersion_changelog.Trim();

                    if (latestVersion != LocalVersion)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Running: V{LocalVersion}");
                        Console.WriteLine($"Update available! Latest version: V{latestVersion}");
                        Console.WriteLine($"V{latestVersion} Changelog: {latestVersion_changelog}");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Press [O] to open the AstralRoyale repository, or any other key to continue...");
                        Console.ResetColor();

                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.O)
                        {
                            try
                            {
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = ProjectPage,
                                    UseShellExecute = true
                                });
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Failed to open the AstralRoyale repository: " + ex.Message);
                                Console.ResetColor();
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You are running the latest version (V{LocalVersion}).");
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to check for updates: " + ex.Message);
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(
                "    _        _             _ ____                   _      \n" +
                "   / \\   ___| |_ _ __ __ _| |  _ \\ ___  _   _  __ _| | ___ \n" +
                "  / _ \\ / __| __| '__/ _` | | |_) / _ \\| | | |/ _` | |/ _ \\\n" +
                " / ___ \\\\__ \\ |_| | | (_| | |  _ < (_) | |_| | (_| | |  __/\n" +
                "/_/   \\_\\___/\\__|_|  \\__,_|_|_| \\_\\___/ \\__, |\\__,_|_|\\___|\n" +
                "                                        |___/                 ");
            Console.WriteLine("RetroRoyale fork by @astralsc");

            Resources.Initialize();

            Configuration.Process();

            if (ServerUtils.IsLinux())
            {
                CommandLoop();
            }
            else
            {
                Logger.Log("Type /help for commands.", null);
                CommandLoop();
            }
        }

        private static void CommandLoop()
        {
            while (_isRunning)
            {
                if (Resources.Configuration.Maintenance && DateTime.UtcNow >= MaintenanceEndTime)
                {
                    Resources.Configuration.Maintenance = false;
                    Resources.Configuration.Save();
                    Resources.Configuration.Initialize();
                    Logger.Log("Maintenance ended automatically.", null);

                    foreach (var player in Resources.Players.Values)
                    {
                        try
                        {
                            int playerId = (int)player.Device.Player.Home.Id;
                            bool isAdmin = ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin(playerId);
                            if (!isAdmin)
                            {
                                new LoginFailedMessage(player.Device)
                                {
                                    Reason = "Maintenance is now over, please reconnect now."
                                }.SendAsync().Wait();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex, null);
                        }
                    }

                    Resources.Players.Clear();
                    Console.WriteLine("All non-admin players may now reconnect.");
                }

                Console.Write("> ");
                string input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input))
                    continue;

                var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var command = parts[0].ToLower();
                string[] args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

                switch (command)
                {
                    case "/help":
                        Logger.Log("Available commands:", null);
                        Logger.Log("/help - Show available commands", null);
                        Logger.Log("/shutdown - Shutdown the server", null);
                        Logger.Log("/maintenance - Enable/Disable maintenance", null);
                        Logger.Log("/key - Generates a random RC4 key", null);
                        break;

                    case "/shutdown":
                        Shutdown().Wait();
                        break;

                    case "/maintenance":
                        HandleMaintenance().Wait();
                        break;

                    case "/status":
                        Logger.Log("Server Status:", null);
                        Logger.Log("Build Version: 1.5 (for 1.9.2)", null);
                        Logger.Log($"Fingerprint SHA: {Resources.Fingerprint.Sha}", null);
                        Logger.Log($"Online Players: {Resources.Players.Count}", null);
                        Logger.Log($"Total Players: {API_Stats.PlayerStat.ToString()}", null);
                        Logger.Log($"Total Clans: {API_Stats.AllianceStat.ToString()}", null);
                        Logger.Log($"1v1 Battles: {Resources.Battles.Count}", null);
                        Logger.Log($"2v2 Battles: {Resources.DuoBattles.Count}", null);
                        Logger.Log($"Tournament Battles: {Resources.TournamentBattles.Count}", null);
                        Logger.Log($"Uptime: {DateTime.UtcNow.Subtract(Resources.StartTime).ToReadableString()}", null);
                        Logger.Log($"Used RAM: {System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024) + " MB"}", null);
                        break;

                    case "/key":
                        Logger.Log($"Generated RC4 Key: {GenerateRc4Key}", null);
                        break;

                    default:
                        Logger.Log($"Unknown command: {input}", null);
                        Logger.Log("Type /help for a list of commands.", null);
                        break;
                }
            }
        }

        public static string GenerateRc4Key
        {
            get
            {
                var random = new Random();
                var token = string.Empty;

                for (var i = 0; i < 38; i++)
                    token += "abcdefghijklmnopqrstuvwxyz0123456789"[random.Next(36)];

                return token;
            }
        }

        private static async Task HandleMaintenance()
        {
            if (!Resources.Configuration.Maintenance)
            {
                Console.WriteLine("Enter maintenance duration in minutes:");
                string input = Console.ReadLine()?.Trim();

                if (!int.TryParse(input, out int minutes) || minutes <= 0)
                {
                    Console.WriteLine("Invalid input.");
                    return;
                }

                Resources.Configuration.Maintenance = true;
                MaintenanceEndTime = DateTime.UtcNow.AddMinutes(minutes);
                Resources.Configuration.Save();
                Resources.Configuration.Initialize();

                Logger.Log($"Maintenance ENABLED for {minutes} minute(s). Ends at {MaintenanceEndTime:u} UTC.", null);

                foreach (var player in Resources.Players.Values)
                {
                    try
                    {
                        int playerId = (int)player.Device.Player.Home.Id;
                        bool isAdmin = ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin(playerId);
                        if (!isAdmin)
                        {
                            await new LoginFailedMessage(player.Device)
                            {
                                ErrorCode = 10,
                                SecondsUntilMaintenanceEnds = (int)(MaintenanceEndTime - DateTime.UtcNow).TotalSeconds
                            }.SendAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex, null);
                    }
                }

                Resources.Players.Clear();
                Console.WriteLine("All non-admin players disconnected.");
            }
            else
            {
                Resources.Configuration.Maintenance = false;
                MaintenanceEndTime = DateTime.UtcNow;
                Resources.Configuration.Save();
                Resources.Configuration.Initialize();

                Logger.Log("Maintenance DISABLED.", null);
                Console.WriteLine("Maintenance has been disabled.");

                foreach (var player in Resources.Players.Values)
                {
                    try
                    {
                        int playerId = (int)player.Device.Player.Home.Id;
                        bool isAdmin = ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin(playerId);
                        if (!isAdmin)
                        {
                            Resources.Configuration.Maintenance = false;
                            Resources.Configuration.Save();
                            Resources.Configuration.Initialize();
                            await new LoginFailedMessage(player.Device)
                            {
                                Reason = "Maintenance is now over, please reconnect now."
                            }.SendAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex, null);
                    }
                }

                Resources.Players.Clear();
                Console.WriteLine("All non-admin players may now reconnect.");
            }
        }

        public static async Task HandleMaintenanceThroughChat(int ChatDaTime)
        {
            if (Resources.Configuration.Maintenance)
            {
                foreach (var player in Resources.Players.Values)
                {
                    try
                    {
                        int playerId = (int)player.Device.Player.Home.Id;
                        bool isAdmin = ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin(playerId);
                        Resources.Configuration.Maintenance = false;
                        Resources.Configuration.Save();
                        Resources.Configuration.Initialize();
                        if (!isAdmin)
                        {
                            await new LoginFailedMessage(player.Device)
                            {
                                Reason = "Maintenance is now over, please reconnect now."
                            }.SendAsync();
                        }
                        Resources.Players.Clear();
                        Logger.Log($"Maintenance has been disabled.", null);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex, null);
                    }
                }
            }
            else
            {
                foreach (var player in Resources.Players.Values)
                {
                    try
                    {
                        int playerId = (int)player.Device.Player.Home.Id;
                        bool isAdmin = ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin(playerId);
                        Resources.Configuration.Maintenance = true;
                        Resources.Configuration.Save();
                        Resources.Configuration.Initialize();
                        MaintenanceEndTime = DateTime.UtcNow.AddMinutes((int)ChatDaTime);
                        if (!isAdmin)
                        {
                            await new LoginFailedMessage(player.Device)
                            {
                                ErrorCode = 10,
                                SecondsUntilMaintenanceEnds = (int)(MaintenanceEndTime - DateTime.UtcNow).TotalSeconds
                            }.SendAsync();
                        }
                        Resources.Players.Clear();
                        Logger.Log($"Maintenance has been enabled.", null);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex, null);
                    }
                }
            }
        }

        public static async Task Shutdown()
        {
            Console.WriteLine("Shutting down...");

            await Resources.Netty.Shutdown();

            try
            {
                Console.WriteLine("Saving players...");
                lock (Resources.Players.SyncObject)
                {
                    foreach (var player in Resources.Players.Values)
                        player.Save();
                }

                Console.WriteLine("All players saved.");
                Exit();
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't save all players.");
                Exit();
            }

            await Resources.Netty.ShutdownWorkers();
        }

        public static void Exit()
        {
            API.Stop();
            _isRunning = false;
            Environment.Exit(0);
        }
    }
}
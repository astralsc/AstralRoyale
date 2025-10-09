using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using ClashRoyale.Logic.Battle;
using ClashRoyale.Logic.Home;

namespace ClashRoyale.Core
{
    public class Configuration
    {
        [JsonIgnore] public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ObjectCreationHandling = ObjectCreationHandling.Reuse,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.None
        };

        // Make sure to edit these on prod
        [JsonIgnore] public static readonly string Version = "1 (stable)";

        [JsonProperty("cluster_encryption_key")]
        public string ClusterKey = "15uvmi8qnyuj9tm53ipaavvytltm582yatecyjzb";

        [JsonProperty("cluster_encryption_nonce")]
        public string ClusterNonce = "nonce";

        [JsonProperty("cluster_server_port")] public int ClusterServerPort = 9876;

        [JsonProperty("encryption_key")] public string EncryptionKey = "fhsd6f86f67rt8fw78fw789we78r9789wer6re";

        [JsonProperty("mysql_database")] public string MySqlDatabase = "ardb";
        [JsonProperty("mysql_password")] public string MySqlPassword = "";
        [JsonProperty("mysql_server")] public string MySqlServer = "127.0.0.1";
        [JsonProperty("mysql_user")] public string MySqlUserId = "root";

        [JsonProperty("patch_url")] public string PatchUrl = "";
        [JsonProperty("sentry_api")] public string SentryApiUrl = "";

        [JsonProperty("server_port")] public int ServerPort = 9339;
        [JsonProperty("update_url")] public string UpdateUrl = "https://github.com/astralsc/AstralRoyale";
        [JsonProperty("use_content_patch")] public bool UseContentPatch;

        [JsonProperty("MinTrophies")] public int MinTroph;
        [JsonProperty("MaxTrophies")] public int MaxTroph;
        [JsonProperty("DefaultGold")] public int DefGold;
        [JsonProperty("DefaultGems")] public int DefGems;
        [JsonProperty("DefaultLevel")] public int DefLevel;

        [JsonProperty("use_udp")] public bool UseUdp;

        [JsonProperty("admins")] public List<long> Admins = new List<long>();
        [JsonProperty("banned_ids")] public List<long> BannedIds = new List<long>();

        [JsonProperty("maintenance")] public bool Maintenance;

        public static void Process()
        {
            try
            {
                var config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json"));

                if (config.DefLevel == 0)
                {
                    Logger.Log(
                        $"DefaultLevel in config.json was set on {config.DefLevel}. Resetting to 1 to prevent crashes.",
                        null
                    );
                    Home.DefaultLevel = 1;
                }
                else if (config.DefLevel >= 14)
                {
                    Logger.Log(
                        $"DefaultLevel in config.json was set on {config.DefLevel}, anything higher than 13 will cause problems. Resetting to 1 to prevent crashes.",
                        null
                    );
                    Home.DefaultLevel = 1;
                }
                else
                {
                    Home.DefaultLevel = config.DefLevel;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex}", null);
                Home.DefaultLevel = 1;
            }
        }

        /// <summary>
        ///     Loads the configuration
        /// </summary>
        public void Initialize()
        {
            if (File.Exists("config.json"))
                try
                {
                    var config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json"));

                    EncryptionKey = config.EncryptionKey;
                    SentryApiUrl = config.SentryApiUrl;

                    MySqlUserId = config.MySqlUserId;
                    MySqlServer = config.MySqlServer;
                    MySqlPassword = config.MySqlPassword;
                    MySqlDatabase = config.MySqlDatabase;

                    PatchUrl = config.PatchUrl;
                    UseContentPatch = config.UseContentPatch;

                    ServerPort = config.ServerPort;
                    UpdateUrl = config.UpdateUrl;

                    MinTroph = config.MinTroph;
                    MaxTroph = config.MaxTroph;
                    DefGems = config.DefGems;
                    DefLevel = config.DefLevel;
                    DefGold = config.DefGold;

                    LogicBattle.MinTrophies = MinTroph;
                    LogicBattle.MaxTrophy = MaxTroph;
                    Home.DefaultGems = DefGems;
                    Home.DefaultLevel = DefLevel;
                    Home.DefaultGold = DefGold;

                    UseUdp = config.UseUdp;

                    Admins = config.Admins;
                    BannedIds = config.BannedIds;

                    Maintenance = config.Maintenance;

                    ClusterServerPort = config.ClusterServerPort;

                    ClusterKey = config.ClusterKey;
                    ClusterNonce = config.ClusterNonce;
                }
                catch (Exception)
                {
                    Console.WriteLine("Couldn't load configuration.");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }
            else
                try
                {
                    Save();

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Server configuration has been created. Restart the server now.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Couldn't create config file.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
        }

        public void Save()
        {
            File.WriteAllText("config.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
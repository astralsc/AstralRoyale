using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using ClashRoyale.Logic;
using ClashRoyale.Logic.Sessions;
using ClashRoyale.Protocol.Messages.Server;
using ClashRoyale.Utilities.Netty;
using DotNetty.Buffers;
using ClashRoyale.Protocol.Messages.Client.Alliance;

namespace ClashRoyale.Protocol.Messages.Client.Login
{
    public class LoginMessage : PiranhaMessage
    {
        private const string SERVER_VERSION_NUMBER = "3.0";

        public LoginMessage(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Id = 10101;
            device.CurrentState = Device.State.Login;
            RequiredState = Device.State.Login;
        }

        public long UserId { get; set; }
        public string UserToken { get; set; }
        public int ClientMajorVersion { get; set; }
        public int ClientBuild { get; set; }
        public int ClientMinorVersion { get; set; }
        public string FingerprintSha { get; set; }
        public string OpenUdid { get; set; }
        public string MacAddress { get; set; }
        public string DeviceModel { get; set; }
        public string AdvertisingGuid { get; set; }
        public string OsVersion { get; set; }
        public byte IsAndroid { get; set; }
        public string AndroidId { get; set; }
        public string PreferredDeviceLanguage { get; set; }

        public override void Decode()
        {
            UserId = Reader.ReadLong();
            UserToken = Reader.ReadScString();

            ClientMajorVersion = Reader.ReadVInt();
            ClientMinorVersion = Reader.ReadVInt();
            ClientBuild = Reader.ReadVInt();

            FingerprintSha = Reader.ReadScString();

            Reader.ReadInt();

            OpenUdid = Reader.ReadScString();
            MacAddress = Reader.ReadScString();
            DeviceModel = Reader.ReadScString();

            AdvertisingGuid = Reader.ReadScString();
            OsVersion = Reader.ReadScString();

            IsAndroid = Reader.ReadByte();

            Reader.ReadScString();

            AndroidId = Reader.ReadScString();
            PreferredDeviceLanguage = Reader.ReadScString().Substring(3, 2);
        }

        public override async void Process()
        {
            if (Resources.Configuration.UseContentPatch)
            {
                if (FingerprintSha != Resources.Fingerprint.Sha)
                {
                    await new LoginFailedMessage(Device)
                    {
                        ErrorCode = 7,
                        ContentUrl = Resources.Configuration.PatchUrl,
                        ResourceFingerprintData = Resources.Fingerprint.Json
                    }.SendAsync();
                    return;
                }
            }

            var player = await Resources.Players.Login(UserId, UserToken);

            if (Resources.Configuration.BannedIds.Contains(UserId))
            {
                await new LoginFailedMessage(Device)
                {
                    ErrorCode = 11
                }.SendAsync();
                return;
            }

            bool missingDeviceModel = string.IsNullOrWhiteSpace(DeviceModel);
            bool missingOsVersion   = string.IsNullOrWhiteSpace(OsVersion);
            bool missingPlayer      = (player == null);

            if (missingDeviceModel && missingOsVersion && missingPlayer)
            {
                if (Device != null)
                {
                    try
                    {
                        Device?.Disconnect();
                        Logger.Log("i am loginmessage.cs i disconnect the ddos.", null);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("ddos protection failed bruh.", null);
                    }
                }
                return;
            }
            
            if (player != null)
            {
                Device.Player = player;
                player.Device = Device;

                var ip = Device.GetIp();

                if (UserId <= 0) player.Home.CreatedIpAddress = ip;

                Device.Player.Home.PreferredDeviceLanguage = PreferredDeviceLanguage;

                var session = Device.Session;
                session.Ip = ip;
                session.GameVersion = $"{ClientMajorVersion}.{ClientMinorVersion}";
                session.Location = await Location.GetByIpAsync(ip);
                session.DeviceCode = DeviceModel;
                session.SessionId = Guid.NewGuid().ToString();
                session.StartDate = session.SessionStart.ToString(CultureInfo.InvariantCulture);

                player.Home.TotalSessions++;

                await new LoginOkMessage(Device).SendAsync();
                await new OwnHomeDataMessage(Device).SendAsync();
                await new AvatarStreamMessage(Device)
                {
                    Entries = player.Home.Stream
                }.SendAsync();

                if (!player.Home.AllianceInfo.HasAlliance) return;

                var alliance = await Resources.Alliances.GetAllianceAsync(player.Home.AllianceInfo.Id);
                if (alliance == null) return;

                Resources.Alliances.Add(alliance);

                await new AllianceStreamMessage(Device)
                {
                    Entries = alliance.Stream
                }.SendAsync();

                alliance.UpdateOnlineCount();

                if (Device.Session.GameVersion != SERVER_VERSION_NUMBER)
                {
                    await new LoginFailedMessage(Device)
                    {
                        ErrorCode = 8,
                        UpdateUrl = Resources.Configuration.UpdateUrl
                    }.SendAsync();
                    return;
                }   

                if (Resources.Configuration.Maintenance)
                {
                    int playerId = (int)Device.Player.Home.Id;
                    bool isAdmin = ClashRoyale.Extensions.Utils.AdminUtils.CheckIfAdmin(playerId);
                    if (!isAdmin)
                    {
                        await new LoginFailedMessage(Device)
                        {
                            ErrorCode = 10,
                            SecondsUntilMaintenanceEnds = (int)(Program.MaintenanceEndTime - DateTime.UtcNow).TotalSeconds
                        }.SendAsync();
                        return;
                    }
                }

                var i = (long)Device.Player.Home.Arena.Trophies;
                if (i < 400)
                {
                    Device.Player.Home.Arena.CurrentArena = 1;
                }
                else
                {
                    if (i >= 400 && i < 800)
                    {
                        Device.Player.Home.Arena.CurrentArena = 2;
                    }
                    else
                    {
                        if (i >= 800 && i < 1100)
                        {
                            Device.Player.Home.Arena.CurrentArena = 3;
                        }
                        else
                        {
                            if (i >= 1100 && i < 1400)
                            {
                                Device.Player.Home.Arena.CurrentArena = 4;
                            }
                            else
                            {
                                if (i >= 1400 && i < 1700)
                                {
                                    Device.Player.Home.Arena.CurrentArena = 5;
                                }
                                else
                                {
                                    if (i >= 1700 && i < 2000)
                                    {
                                        Device.Player.Home.Arena.CurrentArena = 6;
                                    }
                                    else
                                    {
                                        if (i >= 2000 && i < 2300)
                                        {
                                            Device.Player.Home.Arena.CurrentArena = 7;
                                        }
                                        else
                                        {
                                            if (i >= 2300 && i < 2600)
                                            {
                                                Device.Player.Home.Arena.CurrentArena = 8;
                                            }
                                            else
                                            {
                                                if (i >= 2600 && i < 3000)
                                                {
                                                    Device.Player.Home.Arena.CurrentArena = 9;
                                                }
                                                else
                                                {
                                                    if (i >= 3000 && i < 3800)
                                                    {
                                                        Device.Player.Home.Arena.CurrentArena = 10;
                                                    }
                                                    else
                                                    {
                                                        if (i >= 3800 && i < 4000)
                                                        {
                                                            Device.Player.Home.Arena.CurrentArena = 11;
                                                        }
                                                        else
                                                        {
                                                            if (i >= 4000 && i < 4300)
                                                            {
                                                                Device.Player.Home.Arena.CurrentArena = 12;
                                                            }
                                                            else
                                                            {
                                                                if (i >= 4300 && i < 4600)
                                                                {
                                                                    Device.Player.Home.Arena.CurrentArena = 13;
                                                                }
                                                                else
                                                                {
                                                                    if (i >= 4600 && i < 4900)
                                                                    {
                                                                        Device.Player.Home.Arena.CurrentArena = 14;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (i >= 4900 && i < 5200)
                                                                        {
                                                                            Device.Player.Home.Arena.CurrentArena = 15;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (i >= 5200 && i < 5500)
                                                                            {
                                                                                Device.Player.Home.Arena.CurrentArena = 16;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (i >= 5500 && i < 5800)
                                                                                {
                                                                                    Device.Player.Home.Arena.CurrentArena = 17;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (i >= 5800 && i < 6100)
                                                                                    {
                                                                                        Device.Player.Home.Arena.CurrentArena = 18;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (i >= 6100 && i < 6400)
                                                                                        {
                                                                                            Device.Player.Home.Arena.CurrentArena = 19;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (i >= 6400)
                                                                                            {
                                                                                                Device.Player.Home.Arena.CurrentArena = 20;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Automatic Alliance Search
                var buffer = Unpooled.Buffer();
                buffer.WriteInt(0);
                var searchMessage = new SearchAlliancesMessage(Device, buffer);
                searchMessage.Decode();
                await searchMessage.EmptySearch();
            }
            else
            {
                if (Resources.Configuration.Maintenance)
                {
                    await new LoginFailedMessage(Device)
                    {
                        ErrorCode = 10,
                    }.SendAsync();
                    return;
                }
                if (!Resources.Configuration.Maintenance)
                {
                    await new LoginFailedMessage(Device)
                    {
                    Reason = "Account not found. Please clear app data."
                    }.SendAsync();
                    return;
                }
            }
        }
    }
}
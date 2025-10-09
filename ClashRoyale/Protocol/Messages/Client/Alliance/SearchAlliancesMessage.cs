using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClashRoyale.Database;
using ClashRoyale.Logic;
using ClashRoyale.Logic.Clan; // for ClashRoyale.Logic.Clan.Alliance
using ClashRoyale.Protocol.Messages.Server;
using DotNetty.Buffers;

namespace ClashRoyale.Protocol.Messages.Client.Alliance
{
    public class SearchAlliancesMessage : PiranhaMessage
    {
        public string SearchString { get; private set; }

        public SearchAlliancesMessage(Device device, IByteBuffer buffer) : base(device, buffer)
        {
            Id = 14324;
        }

        public override void Decode()
        {
            int length = this.Reader.ReadInt();
            if (length > 0)
            {
                byte[] stringBytes = new byte[length];
                this.Reader.ReadBytes(stringBytes);
                SearchString = System.Text.Encoding.UTF8.GetString(stringBytes);
            }
            else
            {
                SearchString = string.Empty;
            }
        }

        public override async void Process()
        {
            await PerformSearch(SearchString);
        }

        public async Task EmptySearch()
        {
            await PerformSearch(" ");
        }

        private async Task PerformSearch(string search)
        {
            try
            {
                List<ClashRoyale.Logic.Clan.Alliance> allAlliances = await AllianceDb.GetGlobalAlliancesAsync();

                List<ClashRoyale.Logic.Clan.Alliance> filteredAlliances;

                if (string.IsNullOrWhiteSpace(search))
                {
                    filteredAlliances = allAlliances;
                }
                else
                {
                    string searchLower = search.Trim().ToLower();

                    filteredAlliances = allAlliances
                        .Where(clan => !string.IsNullOrEmpty(clan.Name) && clan.Name.ToLower().Contains(searchLower))
                        .ToList();
                }

                await new AllianceListMessage(Device)
                {
                    Alliances = filteredAlliances
                }.SendAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't search any alliance: {ex}");
            }
        }
    }
}
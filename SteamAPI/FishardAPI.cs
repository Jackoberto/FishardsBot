using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace FishardsBot.SteamAPI
{
    class FishardAPI
    {
        private SteamWebInterfaceFactory webInterfaceFactory;
        private SteamUserStats steamUserStats;
        public uint FishardsId => 1637140;
        public uint FishardsDemoId => 1700260;
        public ulong MySteamId => 76561198159249321;
        private static FishardAPI fishardApi;
        private SteamStore steamStore;

        private FishardAPI()
        {
            Init();
        }

        public static FishardAPI GetFishardsAPI()
        {
            fishardApi ??= new FishardAPI();
            return fishardApi;
        }

        async void Init()
        {
            var secrets = await Secrets.GetSecrets();
            webInterfaceFactory = new SteamWebInterfaceFactory(secrets.APIKey);
            steamUserStats = webInterfaceFactory.CreateSteamWebInterface<SteamUserStats>(new HttpClient());
            steamStore = webInterfaceFactory.CreateSteamStoreInterface(new HttpClient());
        }

        public async Task<uint> GetPlayerCount()
        {
            uint playerCount = 0;
            try
            {
                var steamWebResponse = await steamUserStats.GetNumberOfCurrentPlayersForGameAsync(FishardsId);
                playerCount = steamWebResponse.Data;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
                try
                {
                    var steamWebResponse = await steamUserStats.GetNumberOfCurrentPlayersForGameAsync(FishardsDemoId);
                    playerCount = steamWebResponse.Data;
                }
                catch (HttpRequestException exception)
                {
                    Console.WriteLine(exception);
                }
            }

            return playerCount;
        }

        public async Task<string> GetReleaseDate()
        {
            try
            {
                var steamWebResponse = await steamStore.GetStoreAppDetailsAsync(FishardsId);
                return steamWebResponse.ReleaseDate.Date;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    class Secrets
    {
        public string APIKey { get; set; }

        public static async Task<Secrets> GetSecrets()
        {
            var text = await File.ReadAllTextAsync("Secrets.json");
            return JsonSerializer.Deserialize<Secrets>(text);
        }
    }
}
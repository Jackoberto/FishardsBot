using System;
using System.Net.Http;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using FishardsBot.SteamAPI;

namespace FishardsBot.Commands
{
    public class FishardsCommands : BaseCommandModule
    {
        [Command("PlayerCount")]
        [Description("Get the amount of players currently playing Fishards (or Fishards Demo before release) according to Steam")]
        [Aliases("Players")]
        public async Task GetFishardPlayerCount(CommandContext ctx)
        {
            var fishardsAPI = FishardAPI.GetFishardsAPI();
            var playerCount = await fishardsAPI.GetPlayerCount();
            var str = $"Current Player Count: {playerCount}";

            await ctx.Channel.SendMessageAsync(str).ConfigureAwait(false);
        }
        
        [Command("Steam")]
        [Description("Get Steam Link")]
        [Aliases("steamlink", "link", "storepage", "store")]
        public async Task GetSteamLink(CommandContext ctx)
        {
            const string str = "https://store.steampowered.com/app/1637140/Fishards/";
            await ctx.Channel.SendMessageAsync(str).ConfigureAwait(false);
        }
        
        [Command("Release")]
        [Description("Get Release Date")]
        [Aliases("releasedate")]
        public async Task GetReleaseDate(CommandContext ctx)
        {
            var fishardsAPI = FishardAPI.GetFishardsAPI();
            try
            {
                var releaseDate = await fishardsAPI.GetReleaseDate();
                await ctx.Channel.SendMessageAsync($"Planned release date is: {releaseDate}").ConfigureAwait(false);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
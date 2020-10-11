using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace discordbot
{
    class Program
    {
        
        //private static DiscordSocketClient _Client;
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            
            using(var sevices =  ConfigureServices())
            {
                DiscordSocketClient _Client = sevices.GetService<DiscordSocketClient>();
                _Client.Log += _Client_Log;
                await _Client.LoginAsync(Discord.TokenType.Bot,await SupportLib.GetDiscordKey());
                await _Client.StartAsync();
                await sevices.GetRequiredService<BotCommands>().ExecuteAsync();
                await Task.Delay(-1);
                
            }
        }

        private Task _Client_Log(Discord.LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<BotCommands>()
                .AddTransient<HttpClient>()
                .AddSingleton<HackerHelp>()
                .AddSingleton<WeatherHelp>()
                //.AddSingleton<PictureService>()
                .BuildServiceProvider();
        }

    }
    public static class SupportLib //To Get Discord tokens
        {
            public static async Task<string> GetDiscordKey()
            {
                var s  = await GetSupport();
                return s.DiscordKey;
            }
            public static async Task<string> GetWeatherKey()
            {
               var s  = await GetSupport();
                return s.OpenWeatherKey;
            }
            private static async Task<Support> GetSupport()
            {
                Support support;
                using(StreamReader stream = new StreamReader(@"Keys.json"))
               {
                string s = await stream.ReadToEndAsync();
                support = JsonConvert.DeserializeObject<Support>(s);
               }
               return support;
            }
        }
}

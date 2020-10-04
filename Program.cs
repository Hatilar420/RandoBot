using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.WebSocket;
namespace discordbot
{
    class Program
    {

        static async Task Main(string[] args)
        {
            string BotKey = await SupportLib.GetDiscordKey();
            DiscordSocketClient _Client = new DiscordSocketClient();
            _Client.Log += LogMessage;
            await _Client.LoginAsync(Discord.TokenType.Bot, BotKey);
            await _Client.StartAsync();
            await Task.Delay(-1);
        }
      static async Task LogMessage(Discord.LogMessage mess) //Log messages 
      {
         Console.WriteLine(mess.ToString());
      }

    }
    public static class SupportLib //To Get Discord tokens
        {
            public static async Task<string> GetDiscordKey()
            {
                string Key ;
              using(StreamReader stream = new StreamReader(@"Keys.json"))
               {
                string s = await stream.ReadToEndAsync();
                Support j = JsonConvert.DeserializeObject<Support>(s);
                Key = j.DiscordKey;
               }
               return Key;
            }
        }
}

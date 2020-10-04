using System;
using Discord.Commands;
using System.Threading.Tasks;
namespace discordbot
{
    public class NewsCommands : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        public async Task info() => await ReplyAsync("hello world");
    } 
}
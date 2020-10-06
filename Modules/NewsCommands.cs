using System;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net.Http;

namespace discordbot
{
    public class NewsCommands : ModuleBase<SocketCommandContext>
    {
        private readonly HackerHelp _HackerService;
        public NewsCommands(HackerHelp h)
        {
            _HackerService =h;
        }
        [Command("GetTopNews")]
        public async Task info()
        {  
            Story a  = await _HackerService.GetTop();
            await ReplyAsync(a.url);
        }
    } 
}
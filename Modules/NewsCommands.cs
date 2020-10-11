using System;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using System.Collections.Generic;

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
        public async Task info(int N)
        {  
            if(N <= 21 && N > 0)
            {
            List<Story> a  = await _HackerService.GetTop(N);
            EmbedBuilder b = new EmbedBuilder();
            b.Author = new EmbedAuthorBuilder()
            {
                   Name = "HackerNewsBot",
            };
            for(int i = 0 ; i< N; i++)
            {
                b.AddField(name:a[i].by,$"[{a[i].title}]({a[i].url})",true);
            }
            b.Footer = new EmbedFooterBuilder()
            {

                Text = $"Top {N} news . {System.DateTime.UtcNow.ToString()}"
            };
            var c = b.Build();
            await ReplyAsync(embed:c);
            }
            else{
                await ReplyAsync($"Nah");
            }
        }
    } 
}
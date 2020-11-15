using System;
using Discord.Commands;
using Victoria;
using Victoria.Enums;
using System.Threading.Tasks;
using Victoria.EventArgs;
using Discord;

namespace discordbot
{
    public class MusicCommands : ModuleBase<SocketCommandContext>
    {
        private LavaNode _LavaNode;
        private MusicService musicService;
        public MusicCommands(LavaNode l , MusicService ms)
        {
           musicService =ms;
          _LavaNode = l;
        }
        [Command("join")]
        public async Task Join()
        {
          await ReplyAsync(await musicService.Join(Context));
        }



        [Command("play")]
        public async Task Play(params string[] query)
        {
           string q = "";
           foreach(string s in query)
           {
               q+=s;
           }
           await ReplyAsync(await musicService.Play(Context,q));
        }



    [Command("pause")]
    public async Task pause()
    {
        await ReplyAsync(await musicService.Pause(Context));
    }

   [Command("resume")]
    public async Task resume()
    {
        await ReplyAsync(await musicService.Resume(Context));
    }

    [Command("stop")]
    public async Task stop()
    {
        await ReplyAsync(await musicService.Stop(Context));
    }

    [Command("clear")]
    public async Task Clear()
    {
        await ReplyAsync( musicService.Clear(Context));
    }

    [Command("leave")]
    public async Task leave()
    {
        await ReplyAsync(await musicService.leave(Context));
     }
    } 
}
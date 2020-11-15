using System;
using Discord.Commands;
using Victoria;
using Victoria.Enums;
using System.Threading.Tasks;
using Discord;

namespace discordbot
{
    public class MusicCommands : ModuleBase<SocketCommandContext>
    {
        private LavaNode _LavaNode;
        public MusicCommands(LavaNode l)
        {
          _LavaNode = l;
        }
        [Command("join")]
        public async Task Join()
        {
          if(_LavaNode.HasPlayer(Context.Guild))
          {
              await ReplyAsync("Already in a guild");
              return;
          }
          var Voice = Context.User as IVoiceState;
          if(Voice?.VoiceChannel == null)
          {
              await ReplyAsync("Connect to a voice channel");
              return;
          }
          try
          {
              await _LavaNode.JoinAsync(Voice.VoiceChannel,Context.Channel as ITextChannel);
              await ReplyAsync($"joined {Voice.VoiceChannel.Name}");

          }
          catch(Exception e)
          {
              await ReplyAsync($"{e.Message}");
          }
        }

        [Command("play")]
        public async Task Play(params string[] query)
        {
           string q = "";
           foreach(string s in query)
           {
               q+=s;
           }

           if(string.IsNullOrWhiteSpace(q))
           {
               await ReplyAsync("Provide search terms");
               return;
           }

           if(!_LavaNode.HasPlayer(Context.Guild))
           {
                 await ReplyAsync("PLease connect to voice channel");
                 return;
           }

           var search = await _LavaNode.SearchYouTubeAsync(q);
           if(search.LoadStatus == LoadStatus.LoadFailed || search.LoadStatus == LoadStatus.NoMatches)
           {
               await ReplyAsync("Couldn't find anything :(");
               return;
           }
           var player = _LavaNode.GetPlayer(Context.Guild);
           await player.PlayAsync(search.Tracks[0]);
        }
    } 
}
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

           var search = await _LavaNode.SearchAsync(q);
           if(search.LoadStatus == LoadStatus.LoadFailed || search.LoadStatus == LoadStatus.NoMatches)
           {
               await ReplyAsync("Couldn't find anything :(");
               return;
           }
           var player = _LavaNode.GetPlayer(Context.Guild);
           if(player.PlayerState ==  PlayerState.Playing || player.PlayerState == PlayerState.Paused)
           {
               if(!string.IsNullOrWhiteSpace(search.Playlist.Name))
               {
                   foreach(var t in search.Tracks)
                   {
                       player.Queue.Enqueue(t);
                   }
                   await ReplyAsync($"{search.Tracks.Count}");
               }
               else{
                  player.Queue.Enqueue(search.Tracks[0]);
                  await ReplyAsync($"Enqued {search.Tracks[0].Title}");    
               }
           }
           else
           {
               if (!string.IsNullOrEmpty(search.Playlist.Name))
               {
                for(int i = 0 ; i<search.Tracks.Count ; i++)
                {
                    if(i == 0)
                    {
                        await player.PlayAsync(search.Tracks[0]);
                        await ReplyAsync($"Playing {search.Tracks[0].Title}");
                    }
                    else{
                        player.Queue.Enqueue(search.Tracks[i]);
                    }
                }
                await ReplyAsync($"Enqued {search.Tracks.Count}");
               }
               else
               {
                   await player.PlayAsync(search.Tracks[0]);
                   await ReplyAsync($"playing {search.Tracks[0].Title}");
               }
           }
        }



    [Command("pause")]
    public async Task pause()
    {
        if(_LavaNode.HasPlayer(Context.Guild))
        {
           var player = _LavaNode.GetPlayer(Context.Guild);
           if(player.PlayerState == PlayerState.Playing)
           {
               await player.PauseAsync();
           }
           else
           {
             await ReplyAsync("Player is already paused");
           }
        }
        else{
            await ReplyAsync("Please Connect to a voice channel");
        }
    }


   [Command("resume")]
    public async Task resume()
    {
        if(_LavaNode.HasPlayer(Context.Guild))
        {
           var player = _LavaNode.GetPlayer(Context.Guild);
           if(player.PlayerState == PlayerState.Paused)
           {
               await player.ResumeAsync();
           }
           else
           {
             await ReplyAsync("Player is already playing");
           }
        }
        else{
            await ReplyAsync("Please Connect to a voice channel");
        }
    }
    


    [Command("skip")]
    public async Task skip()
    {
        
        if(!_LavaNode.HasPlayer(Context.Guild))
        {
           await ReplyAsync("Please connect to a voice channel");
        }
        var player = _LavaNode.GetPlayer(Context.Guild);
        if(player.Queue.Count < 1)
        {
            await ReplyAsync("Cannot skip one track only use stop instead");
            return;
        }
        else
        {
            try{
            var t =player.Track;
            await player.SkipAsync();
            await ReplyAsync($"Skipped {t.Title}");
            }
            catch(Exception e)
            {
               await ReplyAsync($"{e.Message}");
            }
        }
    }

    [Command("stop")]
    public async Task stop()
    {
        if(_LavaNode.HasPlayer(Context.Guild))
        {
           var player = _LavaNode.GetPlayer(Context.Guild);
           await player.StopAsync();
           await ReplyAsync("Player Stopped playing");
        }
        else{
            await ReplyAsync("Please Connect to a voice channel");
        }
    }


    } 
}
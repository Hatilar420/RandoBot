using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Victoria;
using Victoria.Enums;
 namespace discordbot
 {
     public class MusicService
     {
         private LavaNode _LavaNode;
         public MusicService(LavaNode l)
         {
            _LavaNode = l;
         }

         //Join the voice channel
         public async Task<string> Join(SocketCommandContext Context)
         {
             if(_LavaNode.HasPlayer(Context.Guild))
          {
             return "Already in a guild";
          }
          var Voice = Context.User as IVoiceState;
          if(Voice?.VoiceChannel == null)
          {
              return "Connect to a voice channel";
          }
          try
          {
              await _LavaNode.JoinAsync(Voice.VoiceChannel,Context.Channel as ITextChannel);
             return  $"joined {Voice.VoiceChannel.Name}" ;

          }
          catch(Exception e)
          {
              return $"{e.Message}";
          }
      
         }


      /*
         Play command 
         format: play <url>
     */
      public async Task<string> Play(SocketCommandContext Context , string q)
      {

        if(string.IsNullOrWhiteSpace(q))
           {
               return "Provide search terms";
           }

           if(!_LavaNode.HasPlayer(Context.Guild))
           {
                 return "PLease connect to voice channel";
           }

           var search = await _LavaNode.SearchAsync(q);
           if(search.LoadStatus == LoadStatus.LoadFailed || search.LoadStatus == LoadStatus.NoMatches)
           {
               return "Couldn't find anything :(";
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
                   return  $"{search.Tracks.Count}";
               }
               else{
                  player.Queue.Enqueue(search.Tracks[0]);
                  return $"Enqued {search.Tracks[0].Title}";    
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
                    }
                    else{
                        player.Queue.Enqueue(search.Tracks[i]);
                    }
                }
                 return  $"Playing {search.Tracks[0].Title}"+$"\nEnqued {search.Tracks.Count}";
               }
               else
               {
                   await player.PlayAsync(search.Tracks[0]);
                   return $"playing {search.Tracks[0].Title}";
               }
           }
        
      }


      // To pause the playing music
      public async Task<string> Pause(SocketCommandContext Context)
      {
        if(_LavaNode.HasPlayer(Context.Guild))
        {
           var player = _LavaNode.GetPlayer(Context.Guild);
           if(player.PlayerState == PlayerState.Playing)
           {
               await player.PauseAsync();
               return "Paused";
           }
           else
           {
             return "Player is already paused";
           }
        }
        else{
             return "Please Connect to a voice channel";
        }
      }


      // To Resume the player
      public async Task<string> Resume(SocketCommandContext Context)
      {
          if(_LavaNode.HasPlayer(Context.Guild))
        {
           var player = _LavaNode.GetPlayer(Context.Guild);
           if(player.PlayerState == PlayerState.Paused || player.PlayerState == PlayerState.Stopped)
           {
               await player.ResumeAsync();
               return "Resumed"; 
           }
           else
           {
             return"Player is already playing";
           }
        }
        else{
            return "Please Connect to a voice channel";
        }
      }

    public async Task<string> Skip(SocketCommandContext Context)
    {
         if(!_LavaNode.HasPlayer(Context.Guild))
        {
           return "Please connect to a voice channel";
        }
        var player = _LavaNode.GetPlayer(Context.Guild);
        if(player.Queue.Count < 1)
        {
            return "Cannot skip one track only use stop instead";
        }
        else
        {
            try{
            var t =player.Track;
            await player.SkipAsync();
            return $"Skipped {t.Title}";
            }
            catch(Exception e)
            {
               return $"{e.Message}";
            }
        }
    }

     public async Task<string> Stop(SocketCommandContext Context)
     {
         if(_LavaNode.HasPlayer(Context.Guild))
        {
           var player = _LavaNode.GetPlayer(Context.Guild);
           await player.StopAsync();
           return "Player Stopped playing";
        }
        else{
            return "Please Connect to a voice channel";
        }
     }


        public string Clear(SocketCommandContext Context)
        {
            if (_LavaNode.HasPlayer(Context.Guild))
            {
                var player = _LavaNode.GetPlayer(Context.Guild);
                if (player.Queue.Count > 0)
                {
                    player.Queue.Clear();
                    return "Queue is cleared";
                }
                else
                {
                    return "Queue is already empty";
                }
            }
            else
            {
                return "Please connect to a voice channel";
            }

        }


        public async Task<string> leave(SocketCommandContext Context)
    {
        if(_LavaNode.HasPlayer(Context.Guild))
        {
          var player = _LavaNode.GetPlayer(Context.Guild);
          if(player.PlayerState == PlayerState.Playing)
          {
              await player.StopAsync();
          }
          await _LavaNode.LeaveAsync(player.VoiceChannel);
          return $"Left {Context.Guild.Name}";
        }
         else
          {
              return "Please connect to a voice channel";
          }

    }


    }

}
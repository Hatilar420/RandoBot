using System;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
using Victoria;
using Victoria.EventArgs;
namespace discordbot
{
   public class BotCommands
   { 
       private IServiceProvider _Service ;
       private DiscordSocketClient _Client;
       private CommandService _Commands;
       private LavaNode lavaNode;
       public BotCommands(IServiceProvider _ser,DiscordSocketClient ds, CommandService cs,LavaNode l)
       {
          _Service = _ser;
          _Client = ds;
          _Commands = cs;
          _Client.MessageReceived += Cum;
          _Client.Ready +=Dis;
          lavaNode = l;
          lavaNode.OnTrackEnded += OnEnd;
       }
       public async Task ExecuteAsync()
       {
          await _Commands.AddModulesAsync(Assembly.GetEntryAssembly(),_Service); 
       }
       public async Task Dis()
       {
          if(!lavaNode.IsConnected)
          {
             await lavaNode.ConnectAsync();
          }
       }
      public async Task Cum (SocketMessage message)
      {
          int off =0;
          if (!(message is SocketUserMessage mess)) return;
          if(mess.Source != Discord.MessageSource.User) return;
          if(!mess.HasCharPrefix('!',ref off)) return ;
          var context = new SocketCommandContext(_Client,mess);
          await _Commands.ExecuteAsync(context,off,_Service);
      }

      public async Task OnEnd(TrackEndedEventArgs args)
    {
        if (!args.Reason.ShouldPlayNext()) {
        return;
    }

    var player = args.Player;
    if (!player.Queue.TryDequeue(out var queueable)) {
        await player.TextChannel.SendMessageAsync("Queue completed!");
        return;
    }

    if (!(queueable is LavaTrack track)) {
        await player.TextChannel.SendMessageAsync("Next item in queue is not a track.");
        return;
    }

    await args.Player.PlayAsync(track);
    await args.Player.TextChannel.SendMessageAsync(
        $"{args.Reason}: {args.Track.Title}\nNow playing: {track.Title}");

    }

   }
}
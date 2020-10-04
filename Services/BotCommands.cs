using System;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
namespace discordbot
{
   public class BotCommands
   { 
       private IServiceProvider _Service ;
       private DiscordSocketClient _Client;
       private CommandService _Commands;
       public BotCommands(IServiceProvider _ser,DiscordSocketClient ds, CommandService cs)
       {
          _Service = _ser;
          _Client = ds;
          _Commands = cs;
          _Client.MessageReceived += Cum;
       }
       public async Task ExecuteAsync()
       {
          await _Commands.AddModulesAsync(Assembly.GetEntryAssembly(),_Service); 
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

   }
}
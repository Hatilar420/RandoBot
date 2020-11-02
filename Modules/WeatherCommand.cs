using System;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace discordbot
{
    public class WeatherCommand:ModuleBase<SocketCommandContext>
    {
        private WeatherHelp _WService;
        public WeatherCommand(WeatherHelp wh)
        {
            _WService = wh;
        }
        [Command("weather")]
        public async Task WeatherInfo(params string[] cityname)
        {
           string city= "" ;
           foreach(string s in cityname)
           {
              city += $"{s} "; 
           }
           WeatherSer b =  await _WService.GetCityWeather(city);
           if(b.isValid)
           {
           List<WeatherModel> models= b.weather.ToList<WeatherModel>();
           string Iconurl =  $@"http://openweathermap.org/img/wn/{models[0].icon}@2x.png";
           var e = new EmbedBuilder()
           {
               Author = new EmbedAuthorBuilder()
               {
                   Name = "MadarchodBot",
               },
               Color = Color.Gold,
               ThumbnailUrl = Iconurl
           };
           e.AddField("Current Weather",$"For {city}",false);
           e.AddField("Temprature",$"{Convert.ToDecimal(b.main.temp)-273}",true);
           e.AddField("Description",$"{models[0].description}",true);
           await ReplyAsync(embed:e.Build());
           }
           else
           {
                await ReplyAsync(b.Errors);
           }
        }

    }

}
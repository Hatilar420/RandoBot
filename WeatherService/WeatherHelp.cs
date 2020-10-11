using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace discordbot
{
   public class WeatherHelp
   {
       private HttpClient _Client;
       public WeatherHelp(HttpClient c)
       {
         _Client =c;

       }
       public async Task<WeatherSer> GetCityWeather(string City)
       {
           string ApiKey =  await SupportLib.GetWeatherKey();
           string Url = WeatherUrl.GetCityWeatherUrl(City,ApiKey);
           var  r = await _Client.GetAsync(Url);
           if(r.StatusCode == System.Net.HttpStatusCode.OK)
           {
               try
               {
                WeatherApiBase b =  JsonConvert.DeserializeObject<WeatherApiBase>(await r.Content.ReadAsStringAsync());
                return new WeatherSer(){main = b.main , weather = b.weather ,isValid = true};
               }
               catch(Exception e)
               {
                  return new WeatherSer(){isValid = false , Errors=e.Message};
               }
           }
           else 
           {
               return new WeatherSer{isValid=false,Errors=r.ReasonPhrase,HttpCode=r.StatusCode.ToString()};
           }
       }  

   }
}
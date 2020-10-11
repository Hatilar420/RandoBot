using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace discordbot
{
  
  public static class WeatherUrl
  {    
     public static string GetCityWeatherUrl(string CityName,string ApiKey) => $@"https://api.openweathermap.org/data/2.5/weather?q={CityName}&appid={ApiKey}";
  }
  public class WeatherApiBase
  {
    public IEnumerable<WeatherModel> weather {get;set;}
    public MainWeather main{get;set;}

  }

  public class WeatherSer
  {
    public IEnumerable<WeatherModel> weather {get;set;}
    public MainWeather main{get;set;}
    public bool isValid{get;set;} 
    public string Errors{get;set;}
    public string HttpCode{get;set;}
  }

    public class WeatherModel
    {
        public string id {get;set;}
        public string main {get;set;}
        public string description{get;set;}
        public string icon{get;set;}
    }
    public class MainWeather
    {
       public string temp{get;set;}
       public string feels_like{get;set;}
       public string temp_min{get;set;}
       public string temp_max{get;set;}
       public string pressure{get;set;}
       public string humidity{get;set;}
    }

}
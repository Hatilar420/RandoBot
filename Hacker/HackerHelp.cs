using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace discordbot
{
    public class HackerHelp
    {
        private readonly HttpClient _Client ;
        public HackerHelp(HttpClient c)
        {
           _Client =  c;
        }
       public async Task<List<Story>> GetTop(int n)
       {
           string JsonTopData =  await _Client.GetStringAsync(HackerUrl.TopUrl);
           List<string> tops = JsonConvert.DeserializeObject< List<string> >(JsonTopData);
           List<Story> temp = new List<Story>();
           for(int i = 0 ; i < n ; i++)
           {
                temp.Add(await GetTopStoryAsync(tops[i]));
           }
           return temp;
       }

       public async Task<Story> GetTopStoryAsync (string UserId)
       {
           string UserLink = HackerUrl.GetStoryUrl(UserId);
           string UserData = await _Client.GetStringAsync(UserLink);
           Story UserStory  = JsonConvert.DeserializeObject<Story>(UserData);
           return UserStory;
       }

    }
}
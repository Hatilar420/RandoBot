using System;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace discordbot
{
    public static class HackerUrl
    {
        public const string TopUrl = @"https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty";
        public static string GetStoryUrl(string StoryId) => $@"https://hacker-news.firebaseio.com/v0/item/{StoryId}.json?print=pretty";
    }
    public class HackerTop
    {
      public  IEnumerable<string>  Ids{get;set;}
    }

    public class Story
    {
        public string by {get;set;}
        public string title{get;set;}
        public string type{get;set;}
        public string url{get;set;}
    }

}
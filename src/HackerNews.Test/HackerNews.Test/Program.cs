using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HackerNews.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:19142/");
            HackerNews.Services.Client.BestStoriesClient bestStoriesClient = new Services.Client.BestStoriesClient(httpClient);
            List<HackerNews.Services.Client.BestStoriesResponseMessage> response =  bestStoriesClient.GetAsync().GetAwaiter().GetResult() as List<Services.Client.BestStoriesResponseMessage>;

            foreach(HackerNews.Services.Client.BestStoriesResponseMessage item in response)
            {
                Console.WriteLine("###################################################");
                Console.WriteLine($"Story: {item.Title}");
                Console.WriteLine($"Story Score: {item.Score}");
                Console.WriteLine($"Story PostedBy: {item.PostedBy}");
                Console.WriteLine($"Story Time: {item.Time.ToString()}");
                Console.WriteLine($"Story CommentCount: {item.CommentCount}");
                Console.WriteLine("###################################################");
            }
        }
    }
}

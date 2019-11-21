using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNews.Services.Models
{
    public class StoryItem
    {
        [JsonIgnore]
        public long id { get; set; }
        public string title { get; set; }
        public string uri { get; set; }
        public string postedBy { get; set; }
        public DateTime time { get; set; }
        public int score { get; set; }
        public int commentCount { get; set; }
    }
}

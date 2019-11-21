using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNews.Services.HackerNewsAPI.Models
{
    public class Item
    {
        public long id { get; set; }
        public bool deleted { get; set; }
        public string type { get; set; }
        public string by { get; set; }
        public long time { get; set; }
        public string text { get; set; }
        public bool dead { get; set; }
        public long parent { get; set; }
        public long poll { get; set; }
        public long[] kids { get; set; }
        public string url { get; set; }
        public int score { get; set; }
        public string title { get; set; }
        public long[] parts { get; set; }
        public int descendants { get; set; }


    }
}

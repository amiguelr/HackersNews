using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNews.Services.Models.Translators
{
    public class BestStoriesResponseMessageTranslator
    {
        public static BestStoriesResponseMessage Translate(HackerNewsAPI.Models.Item item)
        {
            BestStoriesResponseMessage retVal = new BestStoriesResponseMessage()
            {
                id = item.id,
                commentCount = item.descendants,
                postedBy = item.by,
                score = item.score,
                time = Utils.UnixTimeStampToDateTime(item.time),
                title = item.title,
                uri = item.url
            };

            return retVal;
        }
    }
}

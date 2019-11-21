using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HackerNews.Services.Models;
using HackerNews.Services.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HackerNews.Services.Controllers
{
    /// <summary>
    /// Story from Hackers News
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private IMemoryCache m_Cache { get; set; }
        private AppSettings m_Settings { get; set; }

        private List<BestStoriesResponseMessage> m_BestStoriesCached;

        private List<BestStoriesResponseMessage> BestStoriesCached
        {
            get
            {
                if (m_BestStoriesCached == null)
                {
                    m_BestStoriesCached = m_Cache.Get("350D0B69-0C61-4ABA-9E02-907C12F3E62A") as List<BestStoriesResponseMessage>;
                    if (m_BestStoriesCached == null) { m_BestStoriesCached = new List<BestStoriesResponseMessage>(); }
                }

                return m_BestStoriesCached;
            }
            set
            {
                m_BestStoriesCached = value;
                m_Cache.Set("350D0B69-0C61-4ABA-9E02-907C12F3E62A", m_BestStoriesCached, DateTimeOffset.Now.AddMinutes(30));
            }
        }

        //dependency injection to cache, settings
        public ItemController(IMemoryCache memoryCache, IOptions<AppSettings> settings)
        {
            m_Cache = memoryCache;
            m_Settings = settings.Value;
        }
        /// <summary>
        /// Get story information
        /// </summary>
        /// <param name="itemId">stoty Id</param>
        /// <returns>BestStoriesResponseMessage</returns>
        [HttpGet("{itemId}")]
        public ActionResult<BestStoriesResponseMessage> Get(long itemId)
        {

            HttpClient client = new HttpClient();
            BestStoriesResponseMessage foundedStory = BestStoriesCached.Find(x => x.id == itemId);
            if (foundedStory == null)
            {
                var request = client.GetStringAsync(m_Settings.Stories.StorieUrl + itemId + ".json");

                var response = request.GetAwaiter().GetResult();
                //Convert to object
                HackerNewsAPI.Models.Item story = JsonConvert.DeserializeObject<HackerNewsAPI.Models.Item>(response);

                //return only stories
                if (story.type.Equals("story", StringComparison.CurrentCultureIgnoreCase))
                {
                    //translate story properties
                    foundedStory = Models.Translators.BestStoriesResponseMessageTranslator.Translate(story) as BestStoriesResponseMessage;
                }
            }

            return foundedStory;
        }
    }
}
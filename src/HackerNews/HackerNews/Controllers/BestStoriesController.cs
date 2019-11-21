using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading.Tasks;
using HackerNews.Services.Models;
using HackerNews.Services.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HackerNews.Services.Controllers
{
    /// <summary>
    /// Controller to Hackers News best stories
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BestStoriesController : ControllerBase
    {
        private IMemoryCache m_Cache { get; set; }
        private ItemController m_ItemController { get; set; }
        private AppSettings m_Settings { get; set; }

        private List<BestStoriesResponseMessage> m_BestStoriesCached;

        private List<BestStoriesResponseMessage> BestStoriesCached
        {
            get 
            {
                if (m_BestStoriesCached == null) { 
                    m_BestStoriesCached = m_Cache.Get("350D0B69-0C61-4ABA-9E02-907C12F3E62A") as List<BestStoriesResponseMessage>;
                    if (m_BestStoriesCached == null) { m_BestStoriesCached = new List<BestStoriesResponseMessage>(); }
                }
                
                return m_BestStoriesCached; 
            }
            set { 
                m_BestStoriesCached = value;
                m_Cache.Set("350D0B69-0C61-4ABA-9E02-907C12F3E62A", m_BestStoriesCached, DateTimeOffset.Now.AddMinutes(30));
            }
        }

        //dependency injection to cache, settings and Item Controller
        public BestStoriesController(IMemoryCache memoryCache, IOptions<AppSettings> settings, ItemController itemController)
        {
            m_Cache = memoryCache;
            m_Settings = settings.Value;
            m_ItemController = itemController;
        }

        /// <summary>
        /// Get 20 best scored stories
        /// </summary>
        /// <returns>List of BestStoriesResponseMessage</returns>
        [HttpGet]
        public ActionResult<List<BestStoriesResponseMessage>> Get()
        {
            List<BestStoriesResponseMessage> retVal = new List<BestStoriesResponseMessage>();

            HttpClient client = new HttpClient();
            var request = client.GetStringAsync(m_Settings.Stories.BestStoriesUrl);

            var response = request.GetAwaiter().GetResult();

            //conver to array all stories id
            long[] stories = JsonConvert.DeserializeObject<long[]>(response);
            bool updateCache = false;
            foreach (long storyId in stories)
            {
                //get item from API or from cache if the storyId exists
                BestStoriesResponseMessage foundedStory = m_ItemController.Get(storyId).Value;

                //if is a non story then is not to include
                if (foundedStory == null) { continue; }
                
                retVal.Add(foundedStory);
                if (!BestStoriesCached.Exists(x => x.id == storyId))
                {
                    updateCache = true;
                    //only affect the private var
                    BestStoriesCached.Add(foundedStory);
                }

            }
            //update catch with the private var
            if(updateCache)
                BestStoriesCached = BestStoriesCached;

            //sort by score desc
            retVal.Sort((x, y) => -1*x.score.CompareTo(y.score));

            //in case if count upper than 20 then remove
            if(retVal.Count>20)
                retVal.RemoveRange(20, retVal.Count-20);

            return retVal;
        }
    }
}

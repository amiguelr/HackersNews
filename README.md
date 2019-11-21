### Features

- Get 20 top scored stories from Hacker News best Stories;
- Get an item story;
- Store in memorycache the stories to best performance, however get always the best stories from source api;
- included project with proxy generated with swagger configuration and generate a nuget to be included into a project.

# HackerNews Solution

HackerNews.Services
-------------
![](https://github.com/amiguelr/HackersNews/blob/master/images/flowdiagram.png)

### Controllers
- BestStoriesController;
- ItemController;

### Run
Open solution and run:

> http://xxxxxx/api/BestStories/
> http://xxxxxx/api/item/1234567

### Install
Install into iis to a website and excute from explorer:

> http://xxxxxx/api/BestStories/
> http://xxxxxx/api/item/1234567
                    
### NSwag
https://github.com/RicoSuter/NSwag


HackerNews.Services.Client
-------------

#### Features
- ProxyGenerated from NSwag Studio (ProxyGenerated.cs)


# HackerNews.Test Solution

#### Features
- Project with an example to call API with nuget objects.

#### Example using nuget HackerNews.Services.Client.1.0.2.nupkg


            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("xxxxx");
            HackerNews.Services.Client.BestStoriesClient bestStoriesClient = new Services.Client.BestStoriesClient(httpClient);
            List<HackerNews.Services.Client.BestStoriesResponseMessage> response =  bestStoriesClient.GetAsync().GetAwaiter().GetResult() as List<Services.Client.BestStoriesResponseMessage>;

    

###End


using HackerNewsModel.Model;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace HackerNewsServices.Service
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _client;
        private readonly IMemoryCache _cache;

        public HackerNewsService(HttpClient client, IMemoryCache cache) {
            _client = client;
            _cache = cache;
        }

        public async Task<List<Story>> GetTopStoryList()
        {
            var cacheKey = "newstories";

            if (_cache.TryGetValue(cacheKey, out List<Story> stories))
            {
                if (stories.Count != 0) {
                    return stories;
                }
                
            }
            try
            {

                var storiesUrl = "https://hacker-news.firebaseio.com/v0/newstories.json?print=pretty";
                var storiesIds = await _client.GetFromJsonAsync<List<int>>(storiesUrl);

                stories = new List<Story>();
                if (storiesIds != null)
                {
                    var paginatedIds = storiesIds.Take(200).ToList();
                    await Task.WhenAll(
                        paginatedIds.Select(async id =>
                        {
                            var storyUrl = $"https://hacker-news.firebaseio.com/v0/item/{id}.json";
                            var story = await _client.GetFromJsonAsync<Story>(storyUrl);
                            if (story != null)
                            {
                                stories.Add(story);
                            }
                        }).ToArray()
                    );
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _cache.Set(cacheKey, stories, cacheEntryOptions);

                return stories.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }   

       
}

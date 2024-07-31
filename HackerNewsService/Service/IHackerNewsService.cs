
using HackerNewsStory.Model;

namespace HackerNewsService.Service
{
    public interface IHackerNewsService
    {
        Task<List<Story>> GetTopStoryList();
       
    }
}

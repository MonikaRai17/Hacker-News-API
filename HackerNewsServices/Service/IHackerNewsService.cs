
using HackerNewsModel.Model;

namespace HackerNewsServices.Service
{
    public interface IHackerNewsService
    {
        Task<List<Story>> GetTopStoryList();
       
    }
}

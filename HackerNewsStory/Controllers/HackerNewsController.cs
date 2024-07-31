
using HackerNewsServices.Service;
using HackerNewsModel.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HackerNewsStory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackernewsservice;
        
        public HackerNewsController(IHackerNewsService hackernewsrepo)
        {
           this._hackernewsservice = hackernewsrepo;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchItem"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetNewsStory(string? searchItem)
        {
            List<Story> allstories = new List<Story>();
            List<Story> filteredstories = new List<Story>();
            try
            {
                var response = await _hackernewsservice.GetTopStoryList();
                if (response == null || response.Count == 0)
                    return NotFound();

                if (response!= null )
                {
                    if (!String.IsNullOrEmpty(searchItem))
                    {
                        var search = searchItem.ToLower();
                        filteredstories = response.Where(s => s.title.ToLower().IndexOf(search) > -1).ToList();
                        if (filteredstories.Count == 0)
                        {
                            return NotFound();
                        }
                        return Ok(filteredstories);
                    }
                    else
                    {
                        return Ok(response);
                    }
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "Issue in News Hacker API!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "exception occured in API");
            }

            
        }

        
       
    }
}

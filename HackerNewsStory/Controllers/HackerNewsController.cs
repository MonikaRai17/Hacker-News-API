
using HackerNewsService.Service;
using HackerNewsStory.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<HackerNews>
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

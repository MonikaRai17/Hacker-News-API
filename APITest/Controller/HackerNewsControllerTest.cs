using HackerNewsStory.Controllers;
using HackerNewsStory.Interface;
using HackerNewsStory.Model;
using HackerNewsStory.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APITest.Controller
{
    public class HackerNewsControllerTest
    {
        private readonly Mock<IHackerNewsRepository> _mockHackerNewsRepo;
        private readonly HackerNewsController _hackernewsController;
        private Mock<IMemoryCache> _cache;
        // Constructor initializes the UserRepository instance
        public HackerNewsControllerTest()
        {
            _cache = new Mock<IMemoryCache>();
            _mockHackerNewsRepo = new Mock<IHackerNewsRepository>();
            _hackernewsController = new HackerNewsController(_cache.Object, _mockHackerNewsRepo.Object);
        }



        [Fact]
        public async void HackerNewsController_GetAllStoriesWithoutSearchItem()
        {
            //Mock<IHackerNewsRepository> _mockHackerNews = new Mock<IHackerNewsRepository>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache
                            .Setup(x => x.CreateEntry(It.IsAny<object>()))
                            .Returns(Mock.Of<ICacheEntry>);
           // HackerNewsController _hackerNewsController = new HackerNewsController(mockMemoryCache.Object,_mockHackerNews.Object );
            var list = new List<Story>();
            _mockHackerNewsRepo.Setup(x => x.GetTopStoryList()).ReturnsAsync(new HttpResponseMessage() { StatusCode = HttpStatusCode.OK });

            var stories = await _hackernewsController.GetNewsStory("");
            list = (stories as OkObjectResult).Value as List<Story>;

            Assert.NotNull(list);
        }

        [Fact]
        public async void HackerNewsController_GetAllStoriesWithSearchItem()
        {
            //Mock<IHackerNewsRepository> _mockHackerNews = new Mock<IHackerNewsRepository>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache
                            .Setup(x => x.CreateEntry(It.IsAny<object>()))
                            .Returns(Mock.Of<ICacheEntry>);
            // HackerNewsController _hackerNewsController = new HackerNewsController(mockMemoryCache.Object,_mockHackerNews.Object );
            var list = new List<Story>();
            _mockHackerNewsRepo.Setup(x => x.GetTopStoryList()).ReturnsAsync(new HttpResponseMessage() { StatusCode = HttpStatusCode.OK });

            var stories = await _hackernewsController.GetNewsStory("Azure");
            list = (stories as OkObjectResult).Value as List<Story>;

            Assert.NotNull(list);
        }








    }
}

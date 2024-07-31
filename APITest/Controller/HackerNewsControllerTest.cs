using HackerNewsStory.Controllers;
using HackerNewsServices.Service;
using HackerNewsModel.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;

 
namespace APITest.Controller
{
    public class HackerNewsControllerTest
    {
        private readonly Mock<IHackerNewsService> _mockHackerNewsService;
        private readonly HackerNewsController _hackernewsController;
        private Mock<IMemoryCache> _cache;
        // Constructor initializes the UserRepository instance
        public HackerNewsControllerTest()
        {
            
            _mockHackerNewsService = new Mock<IHackerNewsService>();
            _hackernewsController = new HackerNewsController(_mockHackerNewsService.Object);
        }



        [Fact]
        public async void HackerNewsController_ReturnsOkResult_WithListOfStories()
        {
            var stories = new List<Story>
            {
                new Story { id = 1, title = "Google DeepMind launches 2B parameter Gemma 2 model", url = "http://abc.com/1" },
                new Story { id = 2, title = "GeoCities Dad Hat", url = "http://xyz.com/2" }
            };

            _mockHackerNewsService.Setup(service => service.GetTopStoryList())
                .ReturnsAsync(stories);

            // Act
            var result = await _hackernewsController.GetNewsStory("");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Story>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async void HackerNewsController_ReturnsOkResult_WithSearchItem()
        {
            var searchItem = "GeoCities";    
            var stories = new List<Story>
            {
                new Story { id = 1, title = "Google DeepMind launches 2B parameter Gemma 2 model", url = "http://abc.com/1" },
                new Story { id = 2, title = "GeoCities Dad Hat", url = "http://xyz.com/2" }
            };

            _mockHackerNewsService.Setup(service => service.GetTopStoryList())
                .ReturnsAsync(stories);

            // Act
            var result = await _hackernewsController.GetNewsStory(searchItem);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Story>>(okResult.Value);
            Assert.Equal(1, returnValue.Count);
        }





    }
}

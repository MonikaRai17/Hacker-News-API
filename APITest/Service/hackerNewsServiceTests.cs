﻿using HackerNewsServices.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Moq.Protected;
using Moq;
using System.Net;
using System.Text.Json;
using HackerNewsModel.Model;

namespace APITest.Service
{
    public class hackerNewsServiceTests
    {
        [Fact]
        public async Task HackerNewsService_ShouldReturnEmpty_WhenApiReturnsNoData()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    if (request.RequestUri.AbsolutePath.EndsWith("newstories.json"))
                    {
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonSerializer.Serialize(new List<int>()))
                        };
                    }
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://hacker-news.firebaseio.com/")
            };

            var mockMemoryCache = new Mock<IMemoryCache>();
            var cacheEntry = new Mock<ICacheEntry>();
            mockMemoryCache
                .Setup(mc => mc.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                .Returns(false);
            mockMemoryCache
                .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
                .Returns(cacheEntry.Object);

           var hackerNewsRepository = new HackerNewsService(httpClient, mockMemoryCache.Object);

            // Act
            var stories = await hackerNewsRepository.GetTopStoryList();

            // Assert
            Assert.NotNull(stories);
            Assert.Empty(stories);
        }

        [Fact]
        public async Task GetNewStoriesAsync_ShouldReturnCachedStories_WhenDataIsCached()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://hacker-news.firebaseio.com/")
            };

            var mockMemoryCache = new Mock<IMemoryCache>();
            var cacheEntry = new Mock<ICacheEntry>();
            var cachedStories = new List<Story>
            {
                new Story { id = 1, title = "Cached Story 1" },
                new Story { id = 2, title = "Cached Story 2" }
            };

            object cacheValue = cachedStories;
            mockMemoryCache
                .Setup(mc => mc.TryGetValue(It.IsAny<object>(), out cacheValue))
                .Returns(true);

           
            var hackerNewsRepository = new HackerNewsService(httpClient, mockMemoryCache.Object);

            // Act
            var stories = await hackerNewsRepository.GetTopStoryList();

            // Assert
            Assert.NotNull(stories);
            Assert.NotEmpty(stories);
            Assert.Equal(2, stories.Count);
            Assert.Equal("Cached Story 1", stories[0].title);
            Assert.Equal("Cached Story 2", stories[1].title);
        }

    }
}

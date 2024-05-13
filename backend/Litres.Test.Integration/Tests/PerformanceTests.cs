using System.Diagnostics;

namespace IntegrationTests.Tests;

public class PerformanceTests(TestingWebAppFactory factory) : IClassFixture<TestingWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Theory]
    [InlineData("api/book/1")]
    [InlineData("api/book/catalog/1/1")]
    [InlineData("api/review/list?bookId=1&n=1")]
    [InlineData("api/subscription/1")]
    public async Task GetMethods_ReturnResponseInExpectedTime(string url)
    {
        // Arrange
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var response = await _client.GetAsync(url);
        stopwatch.Stop();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.True(stopwatch.ElapsedMilliseconds < 1000);
    }
}
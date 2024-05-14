using System.Net;

namespace IntegrationTests.Tests;

public class ApiTests(TestingWebAppFactory factory) : IClassFixture<TestingWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Theory]
    [InlineData("api/book/1")]
    [InlineData("api/book/catalog/1/1")]
    [InlineData("api/review/list?bookId=1&n=1")]
    [InlineData("api/subscription/1")]
    public async Task AnonymousGetMethods_ReturnSuccessStatusCodeWithNotEmptyBody(string url)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Theory]
    [InlineData("api/order/1")]
    [InlineData("api/user/1")]
    [InlineData("api/user/order/list")]
    [InlineData("api/user/settings")]
    public async Task AuthorizedGetMethods_ReturnUnauthorizedStatusCode(string url)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace IntegrationTests.Tests;

using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;


[TestClass]
public class APITest : PlaywrightTest
{
    private IAPIRequestContext Request = null!;

    [TestMethod]
    [DataRow("book/1")]
    [DataRow("book/catalog/1/1")]
    [DataRow("review/1")]
    [DataRow("review/list?bookId=1&page=1")]
    [DataRow("signin/google")]
    [DataRow("subscription/1")]
    [DataRow("user/1")]
    public async Task AnonymousGetMethods_ReturnSuccessStatusCodeWithNotEmptyBody(string url)
    {
        var response = await Request.GetAsync(url);
        await Expect(response).ToBeOKAsync();
        Console.WriteLine(response.BodyAsync());
        
        Assert.NotNull(response.BodyAsync());
    }
    
    [TestInitialize]
    public async Task SetUpAPITesting()
    {
        await CreateAPIRequestContext();
    }

    private async Task CreateAPIRequestContext()
    {

        Request = await Playwright.APIRequest.NewContextAsync(new()
        {
            BaseURL = "http://localhost:5225/api/"
        });
    }
}
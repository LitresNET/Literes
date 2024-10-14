using System.Security.Cryptography.Xml;
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

    [TestMethod]
    [DataRow("signup/user")]
    public async Task SignUpMethodOrSignInIfExist_ReturnSuccessStatusCode(string url)
    {
        var dataSignUp = new Dictionary<string, string>
        {
            { "name", "test" },
            { "email", "test@example.com" },
            { "password", "Test_1234" }
        };
        
        var responseSignUp = await Request.PostAsync(url, new () {DataObject = dataSignUp});

        if (responseSignUp.Ok)
        {
            await Expect(responseSignUp).ToBeOKAsync();
        }
        else
        {
            var dateSignIn = new Dictionary<string, string>
            {
                { "email", "test@example.com" },
                { "password", "Test_1234" }
            };
            
            var responseSignIn = await Request.PostAsync("signin", new() {DataObject = dateSignIn});

            Expect(responseSignIn).ToBeOKAsync();
        }
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
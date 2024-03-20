namespace Tests.MainService.Controllers.RequestControllerTests;

public class DeclinePublishRequest
{
    public async Task DefaultRequest_ReturnsNotPublishedBook_200()
    {
        throw new NotImplementedException();
    }

    public async Task NotExistingRequest_ReturnsRequestNotFoundException_404()
    {
        throw new NotImplementedException();
    }
    
    public async Task DatabaseShut_ReturnsStorageUnavailableException_503()
    {
        throw new NotImplementedException();
    }
}
namespace Tests.MainService.Controllers.RequestControllerTests;

public class DeclinePublishRequest
{
    public async Task DefaultRequest_ReturnsNotPublishedBook_200()
    {
        
    }

    public async Task NotExistingRequest_ReturnsRequestNotFoundException_404()
    {
        
    }
    
    public async Task DatabaseShut_ReturnsStorageUnavailableException_503()
    {
        
    }
}
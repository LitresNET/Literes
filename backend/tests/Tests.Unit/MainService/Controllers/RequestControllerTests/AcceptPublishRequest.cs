namespace Tests.MainService.Controllers.RequestControllerTests;

public class AcceptPublishRequest
{
    public async Task DefaultRequest_ReturnsPublishedBook_200()
    {
        
    }

    public async Task NotExistingRequest_ReturnsRequestNotFoundException_404()
    {
        
    }
    
    public async Task DatabaseShut_ReturnsStorageUnavailableException_503()
    {
        
    }
}
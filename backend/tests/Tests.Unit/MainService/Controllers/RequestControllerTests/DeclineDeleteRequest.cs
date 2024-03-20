namespace Tests.MainService.Controllers.RequestControllerTests;

public class DeclineDeleteRequest
{
    public async Task DefaultRequest_ReturnsNotDeletedBook_200()
    {
        
    }

    public async Task NotExistingRequest_ReturnsRequestNotFoundException_404()
    {
        
    }
    
    public async Task DatabaseShut_ReturnsStorageUnavailableException_503()
    {
        
    }
}
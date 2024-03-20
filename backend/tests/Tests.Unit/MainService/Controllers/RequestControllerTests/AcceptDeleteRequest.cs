namespace Tests.MainService.Controllers.RequestControllerTests;

public class AcceptDeleteRequest
{
    public async Task DefaultRequest_ReturnsDeletedBook_200()
    {
        
    }

    public async Task NotExistingRequest_ReturnsRequestNotFoundException_404()
    {
        
    }
    
    public async Task DatabaseShut_ReturnsStorageUnavailableException_503()
    {
        
    }
}
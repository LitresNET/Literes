namespace Tests.MainService.Controllers.RequestControllerTests;

public class AcceptDeleteRequest
{
    public async Task DefaultRequest_ReturnsDeletedBook_200()
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
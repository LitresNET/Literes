namespace Tests.MainService.Controllers.BookControllerTests;

public class DeleteBook
{
    public async Task DefaultBook_ReturnsRequestDelete_200()
    {
        throw new NotImplementedException();
    }

    public async Task NotExistingBook_ReturnsBookNotFoundException_404()
    {
        throw new NotImplementedException();
    }
    
    public async Task DatabaseShut_ReturnsStorageUnavailableException_503()
    {
        throw new NotImplementedException();
    }
}
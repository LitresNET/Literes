namespace Tests.MainService.Controllers.BookControllerTests;

public class DeleteBook
{
    public async Task DefaultBook_ReturnsRequestDelete_200()
    {
        
    }

    public async Task NotExistingBook_ReturnsBookNotFoundException_404()
    {
        
    }
    
    public async Task DatabaseShut_ReturnsStorageUnavailableException_503()
    {
        
    }
}
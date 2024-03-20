namespace Tests.MainService.Services.BookServiceTests;

public class PublishBook
{
    public async Task DefaultBook_ReturnsRequestCreate()
    {
        
    }

    public async Task EmptyBook_ThrowsBookValidationFailedException()
    {
        
    }

    public async Task BookWithNotExistingAuthor_ThrowsAuthorNotFoundException()
    {
        
    }
    
    public async Task BookWithNotExistingSeries_ThrowsSeriesNotFoundException()
    {
        
    }
    
    public async Task DatabaseShut_ThrowsStorageUnavailableException()
    {
        
    }
}
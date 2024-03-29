﻿namespace IntegrationTests.MainService.Controllers.BookControllerTests;

public class PublishBook
{
    
    public async Task DefaultBook_ReturnsRequestCreate_200()
    {
        throw new NotImplementedException();
    }

    public async Task EmptyBook_ReturnsBookLackRequiredPropertiesException_422()
    {
        throw new NotImplementedException();
    }

    public async Task BookWithNotExistingAuthor_ReturnsAuthorNotFoundException_404()
    {
        throw new NotImplementedException();
    }
    
    public async Task BookWithNotExistingSeries_ReturnsSeriesNotFoundException_404()
    {
        throw new NotImplementedException();
    }
    
    public async Task DatabaseShut_ReturnsStorageUnavailableException_503()
    {
        throw new NotImplementedException();
    }
}
using backend.Abstractions;
using backend.Exceptions;
using backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class RequestService(IRequestRepository requestRepository, IBookRepository bookRepository, IUnitOfWork unitOfWork)
    : IRequestService
{
    public async Task<Book> ChangeBookStateAsync(long requestId, bool requestAccepted = true)
    {
        try
        {
            var request = await requestRepository.GetRequestWithBookByIdAsync(requestId);
            if (request is null)
                throw new RequestNotFoundException(requestId);

            request.Book!.IsApproved = requestAccepted;
            request.Book!.IsAvailable =
                request.RequestType == RequestType.Create
                    ? requestAccepted
                    : request.RequestType == RequestType.Delete && !requestAccepted;

            var bookResult = bookRepository.UpdateBook(request.Book);
            var requestResult = requestRepository.DeleteRequest(request);
            
            await unitOfWork.SaveChangesAsync();
            return bookResult;
        }
        catch (DbUpdateException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }
}

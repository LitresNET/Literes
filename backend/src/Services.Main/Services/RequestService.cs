using backend.Abstractions;
using backend.Exceptions;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class RequestService(
    IRequestRepository requestRepository,
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork
) : IRequestService
{
    public async Task<Book> AcceptPublishDeleteRequestAsync(
        long requestId,
        bool requestAccepted = true
    )
    {
        try
        {
            var request = await requestRepository.GetRequestWithBookByIdAsync(requestId);
            if (request is null || request.RequestType == RequestType.Update)
                throw new RequestNotFoundException(requestId);

            request.Book!.IsApproved = requestAccepted;
            request.Book!.IsAvailable =
                request.RequestType == RequestType.Create ? requestAccepted : !requestAccepted;

            var bookResult = bookRepository.Update(request.Book);
            var requestResult = requestRepository.Delete(request);

            await unitOfWork.SaveChangesAsync();
            return bookResult;
        }
        catch (DbUpdateException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }

    public async Task<Book> AcceptUpdateRequestAsync(long requestId, bool requestAccepted = true)
    {
        try
        {
            var request = await requestRepository.GetRequestWithOldAndUpdatedBooksByIdAsync(requestId);
            if (request is null || request.RequestType != RequestType.Update)
                throw new RequestNotFoundException(requestId);

            request.UpdatedBook!.IsApproved = requestAccepted;
            request.UpdatedBook!.IsAvailable = requestAccepted;

            await bookRepository.DeleteByIdAsync(request.BookId);

            var bookResult = bookRepository.Update(request.UpdatedBook);
            var requestResult = requestRepository.Delete(request);

            await unitOfWork.SaveChangesAsync();
            return bookResult;
        }
        catch (DbUpdateException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }
}

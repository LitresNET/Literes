using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Main.Services;

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
                throw new EntityNotFoundException(typeof(Request), requestId.ToString());

            request.Book!.IsApproved = requestAccepted;
            request.Book!.IsAvailable =
                request.RequestType == RequestType.Create ? requestAccepted : !requestAccepted;

            var bookResult = bookRepository.Update(request.Book);
            requestRepository.Delete(request);

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
                throw new EntityNotFoundException(typeof(Request), requestId.ToString());

            request.UpdatedBook!.IsApproved = requestAccepted;
            request.UpdatedBook!.IsAvailable = requestAccepted;

            request.Book = request.UpdatedBook;
            request.Book.Id = request.BookId;

            await bookRepository.DeleteByIdAsync((long) request.UpdatedBookId);

            var bookResult = bookRepository.Update(request.Book);
            requestRepository.Delete(request);

            await unitOfWork.SaveChangesAsync();
            return bookResult;
        }
        catch (DbUpdateException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }
}

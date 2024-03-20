using backend.Abstractions;
using backend.Models;

namespace backend.Services;

public class RequestService(IRequestRepository requestRepository, IBookRepository bookRepository) : IRequestService
{
    public async Task<Book> AcceptPublishRequestAsync(long requestId)
    {
        var request = await requestRepository.GetRequestWithBookByIdAsync(requestId);
        if (request is null)
            throw new Exception($"No request (id: {requestId} found)");

        request.Book.IsApproved = true;
        request.Book.IsAvailable = true;
        
        var result = bookRepository.UpdateBook(request.Book);
        await bookRepository.SaveChangesAsync();
        return result;
    }

    public async Task<Book> AcceptDeleteRequestAsync(long requestId)
    {
        var request = await requestRepository.GetRequestWithBookByIdAsync(requestId);
        if (request is null)
            throw new Exception($"No request (id: {requestId} found)");

        request.Book.IsApproved = true;
        request.Book.IsAvailable = false;
        
        var result = bookRepository.UpdateBook(request.Book);
        await bookRepository.SaveChangesAsync();
        return result;
    }

    public async Task<Book> DeclinePublishRequestAsync(long requestId)
    {
        var request = await requestRepository.GetRequestWithBookByIdAsync(requestId);
        if (request is null)
            throw new Exception($"No request (id: {requestId} found)");

        request.Book.IsApproved = false;
        request.Book.IsAvailable = false;
        
        var result = bookRepository.UpdateBook(request.Book);
        await bookRepository.SaveChangesAsync();
        return result;
    }

    public async Task<Book> DeclineDeleteRequestAsync(long requestId)
    {
        var request = await requestRepository.GetRequestWithBookByIdAsync(requestId);
        if (request is null)
            throw new Exception($"No request (id: {requestId} found)");

        request.Book.IsApproved = false;
        request.Book.IsAvailable = true;
        
        var result = bookRepository.UpdateBook(request.Book);
        await bookRepository.SaveChangesAsync();
        return result;
    }
}
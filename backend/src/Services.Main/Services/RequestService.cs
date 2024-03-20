using backend.Abstractions;
using backend.Exceptions;
using backend.Models;
using Microsoft.Data.SqlClient;

namespace backend.Services;

public class RequestService(IRequestRepository requestRepository, IBookRepository bookRepository) : IRequestService
{
    public async Task<Book> AcceptPublishRequestAsync(long requestId)
    {
        try
        {
            var request = await requestRepository.GetRequestWithBookByIdAsync(requestId);
            if (request is null)
                throw new RequestNotFoundException(requestId);

            request.Book.IsApproved = true;
            request.Book.IsAvailable = true;

            var result = bookRepository.UpdateBook(request.Book);
            await bookRepository.SaveChangesAsync();
            return result;
        }
        catch (SqlException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }

    public async Task<Book> AcceptDeleteRequestAsync(long requestId)
    {
        try
        {
            var request = await requestRepository.GetRequestWithBookByIdAsync(requestId);
            if (request is null)
                throw new RequestNotFoundException(requestId);

            request.Book.IsApproved = true;
            request.Book.IsAvailable = false;

            var result = bookRepository.UpdateBook(request.Book);
            await bookRepository.SaveChangesAsync();
            return result;
        }
        catch (SqlException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }

    public async Task<Book> DeclinePublishRequestAsync(long requestId)
    {
        try
        {
            var request = await requestRepository.GetRequestWithBookByIdAsync(requestId);
            if (request is null)
                throw new RequestNotFoundException(requestId);

            request.Book.IsApproved = false;
            request.Book.IsAvailable = false;

            var result = bookRepository.UpdateBook(request.Book);
            await bookRepository.SaveChangesAsync();
            return result;
        }
        catch (SqlException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }

    public async Task<Book> DeclineDeleteRequestAsync(long requestId)
    {
        try
        {
            var request = await requestRepository.GetRequestWithBookByIdAsync(requestId);
            if (request is null)
                throw new RequestNotFoundException(requestId);

            request.Book.IsApproved = false;
            request.Book.IsAvailable = true;

            var result = bookRepository.UpdateBook(request.Book);
            await bookRepository.SaveChangesAsync();
            return result;
        }
        catch (SqlException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }
}
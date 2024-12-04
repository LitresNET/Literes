using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;

namespace Litres.Application.Commands.Books.Handlers;

public class DeleteBookCommandHandler(
    IBookRepository bookRepository,
    IRequestRepository requestRepository,
    IMapper mapper
    ) : ICommandHandler<DeleteBookCommand, RequestResponseDto>
{
    public async Task<RequestResponseDto> HandleAsync(DeleteBookCommand command)
    {
        var book = await bookRepository.GetByIdAsync(command.BookId);
        if (book.PublisherId != command.PublisherId)
            throw new PermissionDeniedException($"Delete book {book.Id}");

        book.IsApproved = false;
        book.IsAvailable = false;
        bookRepository.Update(book);

        var request = new Request
        {
            RequestType = RequestType.Delete,
            PublisherId = command.PublisherId,
            Book = book
        };

        var result = await requestRepository.AddAsync(request);
        await requestRepository.SaveChangesAsync();
        
        return mapper.Map<RequestResponseDto>(result);
    }
}
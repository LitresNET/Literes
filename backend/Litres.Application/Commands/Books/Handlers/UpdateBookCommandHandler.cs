using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;

namespace Litres.Application.Commands.Books.Handlers;

public class UpdateBookCommandHandler(
    IBookRepository bookRepository,
    IRequestRepository requestRepository,
    IMapper mapper
    ) : ICommandHandler<UpdateBookCommand, RequestResponseDto>
{
    public async Task<RequestResponseDto> HandleAsync(UpdateBookCommand command)
    {
        //TODO: Нужна ли доп. валидация тут, если она есть на уровне dto?
        var context = new ValidationContext(command);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(command, context, results))
            throw new EntityValidationFailedException(typeof(Book), results);
        
        var book = await bookRepository.GetByIdAsync(command.Id);
        if (book.PublisherId != command.UserId)
            throw new PermissionDeniedException($"Update book {book.Id}");
        //TODO: Это вообще работает? По-моему никакого обновления здесь нет.
        book.IsApproved = false;
        book.IsAvailable = false;
        book.Id = 0;
        await bookRepository.AddAsync(book);

        // при создании запроса на изменение книги, мы хотим, чтобы до одобрения заявки пользователям
        // была доступна старая версия книги. При потдверждении запроса на изменение, старая версия
        // книги удаляется, становится доступна новая
        var request = new Request
        {
            RequestType = RequestType.Update,
            PublisherId = command.UserId,
            Book = book
        };

        var result = await requestRepository.AddAsync(request);
        await requestRepository.SaveChangesAsync();
        
        return mapper.Map<RequestResponseDto>(result);
    }
}
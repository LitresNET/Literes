using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;

namespace Litres.Application.Commands.Books.Handlers;

public class CreateBookCommandHandler(
    IAuthorRepository authorRepository,
    ISeriesRepository seriesRepository,
    IPublisherRepository publisherRepository,
    IRequestRepository requestRepository,
    IMapper mapper
    ) : ICommandHandler<CreateBookCommand, RequestResponseDto>
{
    public async Task<RequestResponseDto> HandleAsync(CreateBookCommand command)
    {
        var book = mapper.Map<Book>(command);
        var context = new ValidationContext(book);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(book, context, results))
            throw new EntityValidationFailedException(typeof(Book), results);

        await authorRepository.GetByIdAsync(book.AuthorId);

        if (book.SeriesId is not null)
            await seriesRepository.GetByIdAsync((long) book.SeriesId);

        if (book.PublisherId is not null)
            await publisherRepository.GetByLinkedUserIdAsync((long) book.PublisherId);

        book.IsApproved = false;
        
        var request = new Request
        {
            RequestType = RequestType.Create,
            PublisherId = (long) book.PublisherId!,
            Book = book
        };

        var requestResult = await requestRepository.AddAsync(request);
        await requestRepository.SaveChangesAsync();

        return mapper.Map<RequestResponseDto>(requestResult);
    }
}
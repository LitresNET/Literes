using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Dto.Responses;
using Litres.Application.Queries.Users;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Users;

public class GetUserPrivateDataQueryHandler(ApplicationDbContext context, IMapper mapper) :
    IQueryHandler<GetUserPrivateData, UserPrivateDataDto>
{
    public async Task<UserPrivateDataDto> HandleAsync(GetUserPrivateData q)
    {
        var user = await context.User.AsNoTracking().FirstOrDefaultAsync(e => e.Id == q.UserId) ?? 
                     throw new EntityNotFoundException(typeof(User), q.UserId.ToString());;
        return mapper.Map<UserPrivateDataDto>(user);
    }
}
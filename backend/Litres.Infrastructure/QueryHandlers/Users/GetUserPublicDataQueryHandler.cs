using AutoMapper;
using Litres.Application.Dto.Responses;
using Litres.Application.Queries.Users;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Users;

public class GetUserPublicDataQueryHandler(ApplicationDbContext context, IMapper mapper) : 
    IQueryHandler<GetUserPublicData, UserPublicDataDto>
{
    public async Task<UserPublicDataDto> HandleAsync(GetUserPublicData q)
    {
        var safeData = await context.User.AsNoTracking()
            .Where(u => u.Id == q.UserId)
            .Select(u => new User
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    AvatarUrl = u.AvatarUrl, 
                    Favourites = u.Favourites, 
                    Reviews = u.Reviews
                }
            )
            .FirstOrDefaultAsync() ??
                       throw new EntityNotFoundException(typeof(User), q.UserId.ToString());;
        return mapper.Map<UserPublicDataDto>(safeData);
    }
}
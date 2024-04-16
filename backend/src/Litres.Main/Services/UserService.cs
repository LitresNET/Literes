using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Litres.Main.Services;

public class UserService(
    IUnitOfWork unitOfWork) : IUserService
{
    public async Task<User> ChangeUserSettingsAsync(User patchedUser)
    {
        var userRepository = unitOfWork.GetRepository<User>();

        var dbUser = await userRepository.GetByIdAsync(patchedUser.Id);
        if (dbUser == null)
            throw new EntityNotFoundException(typeof(User), patchedUser.Id.ToString());

        dbUser.Name = patchedUser.Name;
        dbUser.AvatarUrl = patchedUser.AvatarUrl;

        await unitOfWork.SaveChangesAsync();
        return dbUser;
    }
    
    public async Task<Book> UnFavouriteBookAsync(long userId, long bookIdToDelete)
    {
        var userRepository = unitOfWork.GetRepository<User>();

        var dbUser = await userRepository.GetByIdAsync(userId);
        if (dbUser == null) 
            throw new EntityNotFoundException(typeof(User), userId.ToString());

        var book = dbUser.Favourites.FirstOrDefault(b => b.Id == bookIdToDelete);
        if (book is null)
            throw new EntityNotFoundException(typeof(Book), bookIdToDelete.ToString());
        
        dbUser.Favourites.RemoveAll(b => b.Id == book.Id);
        
        await unitOfWork.SaveChangesAsync();
        return book;
    }
    
    public async Task<User> GetSafeUserDataAsync(long userId)
    {
        var userRepository = (IUserRepository)unitOfWork.GetRepository<User>();
        
        return await userRepository.GetSafeDataById(userId) ?? 
               throw new EntityNotFoundException(typeof(User), userId.ToString());
    }
    
    public async Task<User> GetUserDataAsync(long userId)
    {
        var userRepository = (IUserRepository)unitOfWork.GetRepository<User>();
        return await userRepository.GetByIdAsync(userId) ?? 
               throw new EntityNotFoundException(typeof(User), userId.ToString());
    }

    public async Task<Publisher> GetPublisherAsync(long publisherId)
    {
        var publisherRepository = (IPublisherRepository)unitOfWork.GetRepository<Publisher>();
        
        return await publisherRepository.GetByIdAsync(publisherId) ??
               throw new EntityNotFoundException(typeof(Publisher), publisherId.ToString());
    
    public async Task<string> LoginUserAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            throw new EntityNotFoundException(typeof(User), email);
    
        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        
        if (result == SignInResult.Failed)
            throw new PasswordNotMatchException();
        
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, user.Id.ToString()),
        };
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            var identityRole = await roleManager.FindByNameAsync(role);
            claims.AddRange(await roleManager.GetClaimsAsync(identityRole!));
        }
        if (user.SubscriptionId is not null)
        {
            claims.Add(new Claim(ClaimTypes.Role, user.UserRole.ToString()));
            claims.Add(new Claim(CustomClaimTypes.SubscriptionTypeId, user.SubscriptionId.ToString()!));
            claims.Add(new Claim(CustomClaimTypes.SubscriptionActiveUntil, user.SubscriptionActiveUntil.ToShortDateString()));
        }

        return jwtTokenService.CreateJwtToken(claims);
    }


    public async Task<string> LoginUserFromExternalServiceAsync(string email, IEnumerable<Claim> externalClaims = null)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        //TODO: реализовать логику дорегистрации
        if (user == null)
        {
            /*
            user = new User 
                { 
                    Email = email, 
                    Name = email.Split('@')[0], 
                    UserName = email,
                    PasswordHash = "123destroyMe!"
                };
            await userManager.CreateAsync(user);
            */
            throw new EntityNotFoundException(typeof(User), email);
        }
        
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, user.Id.ToString()),
        };
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            var identityRole = await roleManager.FindByNameAsync(role);
            claims.AddRange(await roleManager.GetClaimsAsync(identityRole!));
        }
        if (user.SubscriptionId is not null)
        {
            claims.Add(new Claim(CustomClaimTypes.SubscriptionTypeId, user.SubscriptionId.ToString()!));
            claims.Add(new Claim(CustomClaimTypes.SubscriptionActiveUntil, user.SubscriptionActiveUntil.ToShortDateString()));
        }
        claims.AddRange(externalClaims);

        return jwtTokenService.CreateJwtToken(claims);
    }
}
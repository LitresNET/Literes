using System.Security.Claims;
using Litres.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Litres.Data.Abstractions.Services;

public interface IUserService
{
    public Task<User> ChangeUserSettingsAsync(User patchedUser);

    public Task<Book> UnFavouriteBookAsync(long userId, long bookIdToDelete);

    public Task<IdentityResult> RegisterUserAsync(User user);

    public Task<IdentityResult> RegisterPublisherAsync(User user, string contractNumber);

    public Task<string> LoginUserAsync(string email, string password);

    public Task<string> LoginUserFromExternalServiceAsync(string email, IEnumerable<Claim> externalClaims = null);
}
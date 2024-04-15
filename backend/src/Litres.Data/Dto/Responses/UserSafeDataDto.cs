using Litres.Data.Models;

namespace Litres.Data.Dto.Responses;

public class UserSafeDataDto
{
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public List<Review> Reviews { get; set; }
    public List<Book> Favourites { get; set; }
}
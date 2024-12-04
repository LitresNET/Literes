namespace Litres.Application.Dto.Responses;

public class UserPublicDataDto
{
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public List<long> Reviews { get; set; }
    public List<long> Favourites { get; set; }
}
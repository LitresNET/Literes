using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

using System.ComponentModel.DataAnnotations;

[Table("User")]
public class User
{
    [Key] 
    public long Id { get; set; }
    [Required]
    public long SubscriptionId { get; set; }
    [Required] 
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Name { get; set; }
    public string AvaterUrl { get; set; }
    public DateTime ActiveUntil { get; set; }
    [Required]
    public bool IsModerator { get; set; }
    [Required]
    public int Wallet { get; set; }

    public List<Book> Purchased { get; set; }
    public List<Book> Favourites { get; set; }
    public List<Review> Reviews { get; set; }
    public List<Comment> Comments { get; set; }
    public List<CommentLike> CommentLikes { get; set; }
    public List<ExternalService> ExternalServices { get; set; }
    public List<Order> Orders { get; set; }
    public Publisher Publisher { get; set; }
    public Subscription Subscription { get; set; }
}
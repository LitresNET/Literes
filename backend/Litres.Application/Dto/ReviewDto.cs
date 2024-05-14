using System.ComponentModel.DataAnnotations;

namespace Litres.Application.Dto;

public class ReviewDto
{
    public long Id { get; set; }
    [MaxLength(4096)]
    public string Content { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public long UserId { get; set; }
    public long? BookId { get; set; }
    public long? ParentReviewId { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public List<ReviewDto>? ChildReviews { get; set; }
}
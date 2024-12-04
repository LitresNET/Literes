using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Reviews;

public record CreateReviewCommand : ICommand<ReviewDto>
{
    public long Id { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public long UserId { get; set; }
    public long? BookId { get; set; }
    public long? ParentReviewId { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public List<ReviewDto>? ChildReviews { get; set; }
}
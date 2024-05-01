namespace Litres.Data.Dto.Responses;

public class ReviewResponseDto
{
    public long Id { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public long UserId { get; set; }
    public long? BookId { get; set; }
    public long? ParentReviewId { get; set; }
}
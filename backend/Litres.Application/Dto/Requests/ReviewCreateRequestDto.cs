using System.ComponentModel.DataAnnotations;

namespace Litres.Application.Dto.Requests;

public class ReviewCreateRequestDto
{
    [MaxLength(4096)]
    public string Content { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    
    public long? BookId { get; set; }
    public long? ParentReviewId { get; set; }
}
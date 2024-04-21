using System.ComponentModel.DataAnnotations;

namespace Litres.Data.Dto.Requests;

public class ReviewChangeRequestDto
{
    [MaxLength(4096)]
    public string Content { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
}
﻿using System.ComponentModel.DataAnnotations;

namespace Litres.Data.Dto.Requests;

public class ReviewCreateRequestDto
{
    [MaxLength(4096)]
    public string Content { get; set; }
    public int Rating { get; set; }
    
    public long? BookId { get; set; }
    public long? ParentReviewId { get; set; }
}
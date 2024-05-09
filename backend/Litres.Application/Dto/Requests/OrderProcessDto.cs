using System.ComponentModel.DataAnnotations;

namespace Litres.Application.Dto.Requests;

public class OrderProcessDto
{
    [Required]
    public string Description { get; set; }
    public Dictionary<long, int> BooksAmount { get; set; }
    public long PickupPointId { get; set; }
}
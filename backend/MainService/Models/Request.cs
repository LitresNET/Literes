using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Request")]
public class Request
{
    [Key]
    public long Id { get; set; }
    [Required]
    public long BookId { get; set; }
    [Required]
    public long RequestTypeId { get; set; }
    [Required]
    public long PublisherId { get; set; }
    
    public Publisher Publisher { get; set; }
    public RequestType RequestType { get; set; }
    public Book Book { get; set; }
}
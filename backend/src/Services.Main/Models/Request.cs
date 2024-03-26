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
    public long UpdatedBookId { get; set; }
    [Required]
    public long PublisherId { get; set; }
    
    public Publisher? Publisher { get; set; }
    [Required]
    public RequestType RequestType { get; set; }
    public Book? Book { get; set; }
    public Book? UpdatedBook { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("RequestType")]
public class RequestType
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string Value { get; set; }
    
    public List<Request> Requests { get; set; }
}
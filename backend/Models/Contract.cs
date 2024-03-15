using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Contract
{
    [Key]
    public long Id { get; set; }
    public string SerialNumber { get; set; }
    
    public Publisher Publisher { get; set; }
}
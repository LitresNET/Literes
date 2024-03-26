using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Publisher
{
    [Key] public long UserId { get; set; }
    [Required] public long ContractId { get; set; }

    public List<Book> Books { get; set; }
    public List<Request> Requests { get; set; }
    public Contract Contract { get; set; }
    public User User { get; set; }

}
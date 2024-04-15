using System.ComponentModel.DataAnnotations;

namespace Litres.Data.Models;

public class Publisher
{
    /// <summary>
    /// Ссылка на договор, заключенный Литрес с издателем
    /// </summary>
    [Required] 
    public long ContractId { get; set; }
    public virtual Contract Contract { get; set; }
    
    /// <summary>
    /// Список выпущенных книг
    /// </summary>
    public virtual List<Book> Books { get; set; }
    
    /// <summary>
    /// Ссылка на юзера, прикрепленного к издателю
    /// </summary>
    [Required]
    [Key]
    public long UserId { get; set; }
    public virtual User User { get; set; }
    
    /// <summary>
    /// Список созданных запросов на изменение состояния книг 
    /// </summary>
    public virtual List<Request> Requests { get; set; }
}
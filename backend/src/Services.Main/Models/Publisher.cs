using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Publisher : User
{
    /// <summary>
    /// Ссылка на договор, заключенный Литрес с издателем
    /// </summary>
    [Required] 
    public long ContractId { get; set; }
    public Contract Contract { get; set; }
    
    /// <summary>
    /// Список выпущенных книг
    /// </summary>
    public List<Book> Books { get; set; }
    
    /// <summary>
    /// Список созданных запросов на изменение состояния книг 
    /// </summary>
    public List<Request> Requests { get; set; }
}
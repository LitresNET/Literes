using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Entities;

[Table("BookOrder")]
public class BookOrder : IEntity
{
    [Key]
    public long Id { get; set; }
    
    // Внешний ключ для заказа
    public long OrderId { get; set; }
    public virtual Order Order { get; set; }
    
    // Внешний ключ для книги
    public long BookId { get; set; }
    public virtual Book Book { get; set; }
    
    // Желаемое количество книги
    public int Quantity { get; set; }
}
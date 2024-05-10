using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Litres.Domain.Abstractions.Entities;
using Litres.Domain.Enums;

namespace Litres.Domain.Entities;

[Table("Order")]
public class Order : IEntity
{
    /// <summary>
    /// Уникальный идентификатор заказа
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// Описание заказа: список всех приобретенных книг
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Статус заказа
    /// </summary>
    [DefaultValue(OrderStatus.Created)]
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Ожидаемое время доставки
    /// </summary>
    public DateTime ExpectedDeliveryTime { get; set; }
    
    /// Оплачен ли заказ
    /// </summary>
    public bool IsPaid { get; set; }

    /// <summary>
    /// Промежуточная таблица для хранения сведений о заказанных книгах, для логики расчета сервиса оплаты
    /// </summary>
    public virtual List<BookOrder> OrderedBooks { get; set; }
    public virtual List<Book> Books { get; set; }

    /// <summary>
    /// Ссылка на пункт выдачи для получения заказа
    /// </summary>
    public long PickupPointId { get; set; }
    public virtual PickupPoint PickupPoint { get; set; }

    /// <summary>
    /// Ссылка на пользователя, оформившего заказ
    /// </summary>
    public long UserId { get; set; }
    public virtual User User { get; set; }
}

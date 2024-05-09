using System.ComponentModel.DataAnnotations;
using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Entities;

public class ExternalService : IEntity
{
    /// <summary>
    /// Уникальный идентификатор стороннего сервиса
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Название стороннего сервиса
    /// </summary>
    [MaxLength(32)]
    public string Name { get; set; }
    
    /// <summary>
    /// Токен, выдаваемый сервису, необходимый для верификации авторизации пользователей
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string ServiceToken { get; set; }
    
    /// <summary>
    /// Пользователи, авторизованные через данный сторонний сервис
    /// </summary>
    public virtual List<User> Users { get; set; }
}
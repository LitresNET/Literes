using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class ExternalService
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
    public List<User> Users { get; set; }
}
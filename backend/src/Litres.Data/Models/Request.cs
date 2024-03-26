using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Litres.Data.Models;

[Table("Request")]
public class Request
{
    /// <summary>
    /// Уникальный идентификатор запроса
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// Тип запроса - создание, удаление и изменение книги
    /// </summary>
    [Required]
    public RequestType RequestType { get; set; }

    /// <summary>
    /// Ссылка на книгу, для изменения состояния которой создан данный запрос
    /// </summary>
    [Required]
    public long BookId { get; set; }
    public Book? Book { get; set; } 
    
    /// <summary>
    /// Ссылка на издателя, инициировавшего изменение состояние книги
    /// </summary>
    [Required]
    public long PublisherId { get; set; }
    public Publisher? Publisher { get; set; }
    
    public long UpdatedBookId { get; set; }
    public Book? UpdatedBook { get; set; }
}
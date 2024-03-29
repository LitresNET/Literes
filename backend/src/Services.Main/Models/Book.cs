using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Book")]
public class Book
{
    /// <summary>
    /// Уникальный идентификатор книги
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Название книги
    /// </summary>
    [MaxLength(128)]
    public string Name { get; set; }
    
    /// <summary>
    /// Описание книги
    /// </summary>
    [MaxLength(4096)]
    public string Description { get; set; }
    
    /// <summary>
    /// Дата публикации книги
    /// </summary>
    [Required]
    public DateTime PublicationDate { get; set; }
    
    /// <summary>
    /// Рейтинг книги, расчитывается из отзывов пользователей
    /// </summary>
    [Required]
    public double Rating { get; set; }
    
    /// <summary>
    /// Ссылка на обложку книги
    /// </summary>
    [MaxLength(256)]
    public string CoverUrl { get; set; }
    
    /// <summary>
    /// Ссылка на текст книги
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string ContentUrl { get; set; }
    
    /// <summary>
    /// Уникальный международный номер ISBN книжного издания 
    /// </summary>
    [MaxLength(17)]
    public string Isbn { get; set; }
    
    /// <summary>
    /// Доступна ли книга для покупки
    /// </summary>
    public bool IsAvailable { get; set; }
    
    /// <summary>
    /// Доступна ли книга для электронного просмотра
    /// </summary>
    [Required]
    public bool IsReadable { get; set; }
    
    /// <summary>
    /// Прошла ли книга все проверки модерации
    /// </summary>
    public bool IsApproved { get; set; }
    
    /// <summary>
    /// Количество оставшихся бумажных копий издания
    /// </summary>
    public int Count { get; set; }
    
    /// <summary>
    /// Цена за один бумажный экземпляр
    /// </summary>
    public int Price { get; set; }
    
    /// <summary>
    /// Список жанров книги
    /// </summary>
    public List<Genre> BookGenres { get; set; }
    
    /// <summary>
    /// Ссылка на автора книги
    /// </summary>
    [Required]
    public long AuthorId { get; set; }
    public Author? Author { get; set; }
    
    /// <summary>
    /// Ссылка на серию книг, если книга относится к таковой
    /// </summary>
    public long? SeriesId { get; set; }
    public Series? Series { get; set; }
    
    /// <summary>
    /// Ссылка на издателя
    /// </summary>
    public long? PublisherId { get; set; }
    public Publisher? Publisher { get; set; }
    
    /// <summary>
    /// Пользователи, добавившие книгу в избранное
    /// </summary>
    public List<User>? Favourites { get; set; } = null;
    
    /// <summary>
    /// Отзывы пользователей на книгу
    /// </summary>
    public List<Review>? Reviews { get; set; }
    
    /// <summary>
    /// Пользователи, приобретшие бумажный экземпляр книги
    /// </summary>
    public List<User>? Purchased { get; set; }
}
namespace Litres.Data.Models;

/// <summary>
/// Тип запроса - создание, удаление или изменение книги
/// </summary>
public enum RequestType
{
    Create = 1,
    Update = 2, 
    Delete = 3,
}
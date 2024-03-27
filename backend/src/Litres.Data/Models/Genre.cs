using System.ComponentModel.DataAnnotations;

namespace Litres.Data.Models;

/// <summary>
/// Список книжных жанров
/// </summary>
public enum Genre
{
    Public = 1,
    
    Action = 2,
    Adventure = 3,
    Autobiography = 4,
    Biography = 5,
    Classic = 6,
    Cookbook = 7,
    Detective = 8,
    Dictionary = 9,
    Encyclopedia = 10,
    Fairytale = 11,
    Fantasy = 12,
    Folklore = 13,
    Graphic = 14,
    Historical = 15,
    Horror = 16,
    RomanceNovel = 17,
    ScienceFiction = 18,
    Textbook = 19,
}
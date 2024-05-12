namespace Litres.Data.Models;

/// <summary>
/// Роль пользователя
/// </summary>
public enum UserRole
{
    //TODO: Поменять. В RoleManager'e у нас роли "Admin", "Publisher", "Member", а тут их на 1 больше (Guest вообще не
    //TODO: нужен, ведь мы никогда не передадим его в Claims) и вместо Member - Regular.
    Guest,
    Regular,
    Publisher,
    Admin
}
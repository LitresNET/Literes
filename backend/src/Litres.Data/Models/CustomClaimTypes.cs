namespace Litres.Data.Models;

/// <summary>
/// Класс, расширяющий базовый класс System.Security.Claims.ClaimTypes 
/// </summary>
public class CustomClaimTypes
{
    /// <summary>
    /// Идентификатор пользователя в базе данных
    /// </summary>
    public const string UserId = "User Id";

    /// <summary>
    /// Роль пользователя в системе
    /// </summary>
    public const string UserRole = "User role";
    
    /// <summary>
    /// Идентификатор текущей подписки пользователя
    /// </summary>
    public const string SubscriptionTypeId = "Subscription Type Id";

    /// <summary>
    /// Дата окончания подписки
    /// </summary>
    public const string SubscriptionActiveUntil = "Subscription Active Until";
}
namespace Litres.Data.Models;

/// <summary>
/// Класс, расширяющий базовый класс System.Security.Claims.ClaimsTypes 
/// </summary>
public class CustomClaimTypes
{
    /// <summary>
    /// Идентификатор пользователя в базе данных
    /// </summary>
    public const string UserId = "User Id";

    public const string SubscriptionTypeId = "Subscription Type Id";

    public const string SubscriptionActiveUntil = "Subscription Active Until";
}
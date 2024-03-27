namespace Litres.Data.Abstractions.Repositories;

/// <summary>
/// В EntityFramework есть особенность - метод SaveChanges можно вызвать только 1 раз в рамках
/// одного объекта. Поэтому, если в одном сервисе есть взаимодействие с несколькими репозиториями,
/// может произойти ошибка, т.к. в ASP.NET объект DbContext по умолчанию создается с временем жизни Scoped
/// Чтобы предотвратить эту ошибку, все репозитории должны наследоваться от этого базового репозитория
/// Тем самым сохранение изменений в бд делегируется не репозиторию, а сервису
/// </summary>

//TODO: Добавить IAsyncDisposable
public interface IUnitOfWork : IDisposable
{
    public Task SaveChangesAsync();
}

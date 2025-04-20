using System.Data;
using System.Text.Json;
using Dapper;

namespace Orders.Api.Outbox;

internal static class OutboxExtensions
{
    internal static async Task InsertOutboxMessage<T>(
        this IDbConnection connection,
        T message,
        IDbTransaction? transaction = default)
        where T : notnull
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = message.GetType().FullName!,
            Content = JsonSerializer.Serialize(message),
            OccuredOnUtc = DateTime.UtcNow
        };

        const string sql =
            """
            INSERT INTO outbox_messages (id, type, content, occured_on_utc)
            values (@Id, @Type, @Content::jsonb, @OccuredOnUtc)
            """;

        await connection.ExecuteAsync(sql, outboxMessage, transaction: transaction);
    }
}
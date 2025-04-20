using System.Text.Json;
using Dapper;
using MassTransit;
using Npgsql;

namespace Orders.Api.Outbox;

internal sealed class OutboxProcessor(NpgsqlDataSource dataSource, IPublishEndpoint publishEndpoint)
{
    private const int BatchSize = 10;

    public async Task<int> Execute(CancellationToken ctx = default)
    {
        await using var connection = await dataSource.OpenConnectionAsync(ctx);
        await using var transaction = await connection.BeginTransactionAsync(ctx);

        var outboxMessages = (await connection.QueryAsync<OutboxMessage>(
            """
            select *
            from outbox_messages
            where processed_on_utc is null
            order by occured_on_utc limit @BatchSize
            """,
            new {BatchSize},
            transaction: transaction)).AsList();

        foreach (var outboxMessage in outboxMessages)
        {
            try
            {
                var messageType = Messaging.Contracts.AssemblyReference.Assembly.GetType(outboxMessage.Type)!;
                var deserializedMessage = JsonSerializer.Deserialize(outboxMessage.Content, messageType)!;

                await publishEndpoint.Publish(deserializedMessage, messageType, ctx);

                await connection.ExecuteAsync(
                    """
                    update outbox_messages
                    set processed_on_utc = @ProcessedOnUtc
                    where id = @Id
                    """,
                    new {ProcessedOnUtc = DateTime.UtcNow, outboxMessage.Id},
                    transaction: transaction);
            }
            catch (Exception ex)
            {
                await connection.ExecuteAsync(
                    """
                    update outbox_messages
                    set processed_on_utc = @ProcessedOnUtc, error = @Error
                    where id = @Id
                    """,
                    new {ProcessedOnUtc = DateTime.UtcNow, Error = ex.ToString(), outboxMessage.Id},
                    transaction: transaction);
            }
        }

        await transaction.CommitAsync(ctx);

        return outboxMessages.Count;
    }
}
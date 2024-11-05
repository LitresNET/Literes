using Litres.ChatApp;

ConsoleMessage.WelcomeMessage();
ChatUtils.PerformAuthentication();

var connection = ChatUtils.NewConnection();
if (connection is null) return;

try
{
    await connection.StartListening();
}
catch (Exception ex)
{
    ConsoleMessage.Error($"Ошибка подключения: {ex.Message}");
}
finally
{
    await connection.StopListening();
}
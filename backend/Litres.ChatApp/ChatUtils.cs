using Litres.Application.Dto.Requests;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.SignalR.Client;

namespace Litres.ChatApp;

public static class ChatUtils
{
    private static String _jwtToken = "";
    private static String _userName = "";
    private static String _userEmail = "";
    private static String _chatSessionId = "";
    
    private static void ConfigureOnNewMessage(this HubConnection connection)
    {
        connection.On<Message>("ReceiveMessage", message =>
        {
            ConsoleMessage.NewMessage(message);
            _chatSessionId = message.ChatSessionId;
        });
    }

    private static UserRegistrationDto GetRegistrationInfo()
    {
        var dto = new UserRegistrationDto();
        ConsoleMessage.RegistrationMessage();

        Console.Write("Введите ваше имя: ");
        dto.Name = Console.ReadLine();
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Введите ваш email: ");
        Console.ResetColor();
        dto.Email = Console.ReadLine();
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Придумайте пароль: ");
        Console.ResetColor();
        dto.Password = Console.ReadLine();
        
        _userName = dto.Name;
        _userEmail = dto.Email;
        
        return dto;
    }

    private static UserLoginDto GetLoginInfo()
    {
        var dto = new UserLoginDto();
        ConsoleMessage.LoginMessage();

        if (String.IsNullOrEmpty(_userEmail))
        {
            Console.Write("Введите ваш email для входа: ");
            dto.Email = Console.ReadLine();
            _userEmail = dto.Email;
        }
        else
        {
            Console.WriteLine($"Используется сохранённый email: {_userEmail}");
        }
        
        Console.Write("Введите ваш пароль для входа: ");
        dto.Password = Console.ReadLine();
        
        return dto;
    }

    
    public static HubConnection? NewConnection()
    {
        var signalRUrl = "http://localhost:5225/api/hubs/chat";
        var connection = new HubConnectionBuilder()
            .WithUrl(signalRUrl + "?token=" + _jwtToken, options =>
            { 
                options.AccessTokenProvider = () => Task.FromResult(_jwtToken);
                options.Headers["Authorization"] = "Bearer " + _jwtToken;
            })
            .Build();
        connection.ConfigureOnNewMessage();
        return connection;
    }
    
    public static async Task StartListening(this HubConnection connection)
    {
        await connection.StartAsync();
        ConsoleMessage.StartMessage();
    
        while (true)
        {
            var text = Console.ReadLine();
            if (text == "exit") break;
        
            var message = new Message()
            {
                From = "Agent",
                SentDate = DateTime.Now,
                Text = text,
                ChatSessionId = _chatSessionId
            };
            ConsoleMessage.MyMessage(message);
            await connection.SendAsync("SendMessageAsync", message);
        }
    }

    public static async Task PerformAuthentication()
    {
        var repeatCommand = true;
        while (repeatCommand)
        {
            ConsoleMessage.AuthenticateMessage();
            var command = Console.ReadLine();
            switch (command)
            {
                case "register agent":
                    try
                    {
                        var registrationInfo = GetRegistrationInfo();
                        var token = await SignInUtils.SignUp(registrationInfo);
                        _jwtToken = token;
                        repeatCommand = false;
                    }
                    catch (Exception e)
                    {
                        ConsoleMessage.Error(e.Message);
                        ConsoleMessage.Error(e.ToString());
                        ConsoleMessage.Error(e.StackTrace);
                    }
                    break;
                case "sign in":
                    try
                    {
                        var loginInfo = GetLoginInfo();
                        var token = await SignInUtils.SignIn(loginInfo);
                        _jwtToken = token;
                        repeatCommand = false;
                    }
                    catch (Exception e)
                    {
                        ConsoleMessage.Error(e.Message);
                    }
                    break;
                case "exit":
                    ConsoleMessage.FinalMessage();
                    return;
            }
        }
    }
    
    public static async Task StopListening(this HubConnection connection)
    {
        ConsoleMessage.FinalMessage();
        await connection.StopAsync();
    }
}
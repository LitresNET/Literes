using Litres.Domain.Entities;

namespace Litres.ChatApp;

public static class ConsoleMessage
{
    public static void WelcomeMessage()
    {
        Console.Clear();
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("*******************************************");
        Console.WriteLine("   Добро пожаловать в чат поддержки Litres!     ");
        Console.WriteLine("*******************************************");
        Console.ResetColor();
    }
    
    public static void StartMessage()
    { 
        Console.ResetColor();
        Console.WriteLine("*******************************************");
        Console.WriteLine("Вы вышли в Internet...");
        Console.WriteLine("*******************************************");
    }
    
    public static void FinalMessage()
    { 
        Console.ResetColor();
        Console.WriteLine("*******************************************");
        Console.WriteLine("Вы покидаете Internet...");
        Console.WriteLine("*******************************************");
    }

    public static void AuthenticateMessage()
    {
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nДля продолжения работы с чатом, пожалуйста, войдите в систему");
        Console.WriteLine("Если вы хотите стать агентом поддержки, введите команду: 'register agent'.");
        Console.WriteLine("Если вы уже зарегистрированы как агент, введите команду: 'sign in'.");
        Console.WriteLine("Для выхода, введите команду: 'exit' или нажмите Ctrl + C.");
    }

    public static void RegistrationMessage()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("*******************************************");
        Console.WriteLine("   Регистрация нового пользователя       ");
        Console.WriteLine("*******************************************");
        Console.ResetColor();
    }

    public static void LoginMessage()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n*******************************************");
        Console.WriteLine("     Вход в систему                      ");
        Console.WriteLine("*******************************************");
        Console.ResetColor();
    }
    
    public static void NewMessage(Message message)
    {
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("*******************************************");
        Console.WriteLine($"У вас (1) новое сообщение:");
        Console.WriteLine("*******************************************");
        
        Console.ResetColor();
        Console.WriteLine($"{message.SentDate} | {message.From}: {message.Text}");
    }
    
    public static void MyMessage(Message message)
    {
        Console.ResetColor();
        Console.WriteLine($"{message.SentDate} | Вы: {message.Text}");
    }
    
    public static void ErrorNoConnection()
    {
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ошибка: Переменная окружения SIGNALR_URL не установлена.");
    }

    public static void Error(string message)
    {
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
    }
}
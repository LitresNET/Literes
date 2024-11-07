using System.Net.Http.Json;
using Litres.Application.Dto.Requests;

namespace Litres.ChatApp;

public static class SignInUtils
{ 
    private static readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5225")
    };
    
    public static async Task<String> SignUp(UserRegistrationDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/signup/agent", dto); 
        if (response.IsSuccessStatusCode)
        {
            var loginDto = new UserLoginDto
            {
                Email = dto.Email,
                Password = dto.Password
            };
            
            return await SignIn(loginDto);
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ошибка при регистрации: {error}");
        }
    }
    
    public static async Task<String> SignIn(UserLoginDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/signin", dto);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"token: {result}");
            return result ?? throw new Exception("Не удалось получить токен.");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ошибка при входе: {error}");
        }
    }
}
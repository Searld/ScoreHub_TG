using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ScoreHub_TG;

public class API
{
    private static string _token = "";
    public static async Task<bool> VerifyData(string userId)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"https://localhost:44340/verify-tg/{userId}");
        var statusCode = response.StatusCode;
        if(statusCode == HttpStatusCode.OK)
        {
            _token = await response.Content.ReadAsStringAsync();
            return true;
        }
        return false;
    }

    public static async Task<UserResponse> GetUser(string userId)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        var response = await httpClient.GetAsync($"https://localhost:44340/user/{userId}");
        string json = await response.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true 
        });
        return userResponse;
    }
}
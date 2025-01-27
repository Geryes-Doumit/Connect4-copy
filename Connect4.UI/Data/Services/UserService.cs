using System;
using System.Text;
using System.Text.Json;
using Connect4.Common.Model;

namespace Connect4.UI.Data.Services;

public class UserService(HttpClient client) : ServiceBase(client, "User")
{
    public async Task<HttpResponseMessage> SendLoginRequest(string username, string password)
    {
        var request = MakeBasicRequest(HttpMethod.Post, Endpoint + "/Login");
        var tempStr = $"{username}:{password}";
        request.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(tempStr))}");
        

        var response = await client.SendAsync(request);
        return response;
    }
    // JsonSerializer.DeserializeAsync<LoginResponseDto>(responseStream, _options);

    public async Task<HttpResponseMessage> SendLogoutRequest(string token)
    {
        var request = MakeBasicRequest(HttpMethod.Post, Endpoint + "/Logout");
        request.Headers.Add("Authorization", $"Bearer {token}");
        
        var response = await client.SendAsync(request);
        return response;
    }
    // JsonSerializer.DeserializeAsync<MessageResponsDto>(responseStream, _options);
}
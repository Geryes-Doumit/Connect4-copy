using System.Text;
using System.Text.Json;
using Connect4.Common.Model;

namespace Connect4.UI.Data.Services;

public class ServiceBase(HttpClient client, string controller) 
{
    private const string BASE_URL = "https://localhost:6969/api/";
    public static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    protected string Endpoint => BASE_URL + controller;

    protected static HttpRequestMessage MakeBasicRequest(HttpMethod method, string uri)
    {
        var request = new HttpRequestMessage(method, uri);

        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("User-Agent", "Connect4");
        request.Headers.Add("Access-Control-Allow-Origin", "*");

        return request;
    }
    
    public HttpRequestMessage MakeGetWaitingGamesRequest(string token, int limit, int offset)
    {
        var request = MakeBasicRequest(HttpMethod.Get, Endpoint + "/Waiting");
        request.Headers.Add("Authorization", $"Bearer {token}");
        request.Headers.Add("limit", limit.ToString());
        request.Headers.Add("offset", offset.ToString());
        
        return request;
    }
}
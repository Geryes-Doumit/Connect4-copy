using System.Text;
using System.Text.Json;

namespace Connect4.UI.Data.Services;

public class InGameService(HttpClient client) : ServiceBase(client, "Game")
{

    public async Task<HttpResponseMessage> SendJoinGameRequest(
        string token, int gameId
    ) {
        var reqStr = $"/{gameId}/join";
        var request = MakeBasicRequest(HttpMethod.Post, Endpoint + reqStr);
        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await client.SendAsync(request);
        return response;
    }

    public async Task<HttpResponseMessage> SendGetGameDetailsRequest(
        string token, int gameId
    ) {
        var reqStr = $"/{gameId}";
        var request = MakeBasicRequest(HttpMethod.Get, Endpoint + reqStr);
        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await client.SendAsync(request);
        return response;
    }

    public async Task<HttpResponseMessage> SendPlayMoveRequest(
        string token, int gameId, int column
    ) {
        var reqStr = $"/{gameId}/playMove";
        var request = MakeBasicRequest(HttpMethod.Post, Endpoint + reqStr);
        request.Headers.Add("Authorization", $"Bearer {token}");
        request.Content = new StringContent(
            column.ToString(),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.SendAsync(request);
        return response;
    }

    public async Task<HttpResponseMessage> SendLeaveGameRequest(
        string token, string gameId
    ) {
        var reqStr = $"/{gameId}/leave";
        var request = MakeBasicRequest(HttpMethod.Post, Endpoint + reqStr);
        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await client.SendAsync(request);
        return response;
    }

    public async Task<HttpResponseMessage> SendCreateGameRequest(
        string token, string gameName
    ) {
        var request = MakeBasicRequest(HttpMethod.Post, Endpoint);
        request.Headers.Add("Authorization", $"Bearer {token}");
        request.Content = new StringContent(
            $"\"{gameName}\"",
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.SendAsync(request);
        return response;
    }
}

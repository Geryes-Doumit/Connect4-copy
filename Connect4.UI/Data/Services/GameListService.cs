using System;
using Connect4.Common.Model;
using Connect4.UI.Pages.Connect4Pages;

namespace Connect4.UI.Data.Services;

public class GameListService(HttpClient client) : ServiceBase(client, "GamesList")
{
    public async Task<HttpResponseMessage> SendGetGamesListRequest(
        string token, int limit, int offset
    ) {
        var reqStr = $"/Waiting?limit={limit}&offset={offset}";

        var request = MakeBasicRequest(HttpMethod.Get, Endpoint + reqStr);
        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await client.SendAsync(request);
        return response; 
    }
    // JsonSerializer.DeserializeAsync<WaitingGameDto[]>(responseStream, _options);

    public async Task<HttpResponseMessage> SendGetGameHistoryRequest(
        string token, int limit, int offset
    ) {
        var reqStr = $"/History?limit={limit}&offset={offset}";

        var request = MakeBasicRequest(HttpMethod.Get, Endpoint + reqStr);
        request.Headers.Add("Authorization", $"Bearer {token}");
        
        var response = await client.SendAsync(request);
        return response;
    }
    // JsonSerializer.DeserializeAsync<FinishedGameDto[]>(responseStream, _options);
}

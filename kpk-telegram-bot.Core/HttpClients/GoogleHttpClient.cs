using kpk_telegram_bot.Common.Contracts.HttpClients;
using Newtonsoft.Json;

namespace kpk_telegram_bot.Core.HttpClients;

public class GoogleHttpClient : IGoogleHttpClient
{
    private HttpClient _client;

    public GoogleHttpClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<TResult> GetAsync<TResult>(HttpRequestMessage httpRequest)
    {

        //await LogRequest(httpRequest);
        var response = await _client.SendAsync(httpRequest);
        //await LogResponse(response);
        try
        {
            response.EnsureSuccessStatusCode();
            var rawContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(rawContent);
        }
        catch (Exception ex)
        {
           // await LogRequest(httpRequest, LogEventLevel.Error);
           // await LogResponse(response, LogEventLevel.Error);
            throw;
        }
    }
}
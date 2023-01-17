namespace kpk_telegram_bot.Common.Contracts.HttpClients;

public interface IGoogleHttpClient
{
    Task<TResult> GetAsync<TResult>(HttpRequestMessage httpRequest);
}
using System.Net.Http.Headers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Models;
using kpk_telegram_bot.Common.Options;
using Telegram.Bot.Types;

namespace kpk_telegram_bot.Core.Commands;

public class MyMarksCommand : ICommand
{
    private readonly string _applicationName;
    private readonly string _username;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _accessToken;
    private readonly string _apiKey;
    private readonly IGoogleHttpClient _googleHttpClient;

    public MyMarksCommand(ApplicationOptions appOptions, GoogleDriveApiOptions driveOptions, IGoogleHttpClient googleHttpClient)
    {
        _applicationName = appOptions.ApplicationName ?? throw new ArgumentNullException(appOptions.ApplicationName);
        _username = appOptions.Username ?? throw new ArgumentNullException(appOptions.Username);
        _clientSecret = driveOptions.ClientSecret ?? throw new ArgumentNullException(driveOptions.ClientSecret);
        _clientId = driveOptions.ClientID ?? throw new ArgumentNullException(driveOptions.ClientID);
        _accessToken = driveOptions.AccessToken ?? throw new ArgumentNullException(driveOptions.AccessToken);
        _apiKey = driveOptions.ApiKey ?? throw new ArgumentNullException(driveOptions.ApiKey);
        _googleHttpClient = googleHttpClient;
    }
    
    public async Task Execute(Message message)
    {
        var url = $"https://content-sheets.googleapis.com/v4/spreadsheets/{Sheets.test}/values:batchGet?ranges=A1%3AB2&key={_apiKey}";
        var httpRequest = new HttpRequestMessage
        {
            RequestUri = new Uri(url, UriKind.RelativeOrAbsolute),
            Method = HttpMethod.Get,
        };
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        httpRequest.Headers.Add("referer", "https://content-sheets.googleapis.com/static/proxy.html?usegapi=1&jsh=m%3B%2F_%2Fscs%2Fabc-static%2F_%2Fjs%2Fk%3Dgapi.lb.en.geaHZXF2-fw.O%2Fd%3D1%2Frs%3DAHpOoo9yYF5eCIYPx4UH9gpJptM2Q_GGxQ%2Fm%3D__features__");
        httpRequest.Headers.Add("x-origin", "https://explorer.apis.google.com");
        httpRequest.Headers.Add("x-referer", "https://explorer.apis.google.com");
        httpRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
        httpRequest.Headers.Add("athority", "content-sheets.googleapis.com");
        
        var res = await _googleHttpClient.GetAsync<SheetModel>(httpRequest);
        
    }
    
    private async Task<SheetsService> Initialize()
    {
        var clientSecrets = new ClientSecrets
        {
            ClientId = _clientId,
            ClientSecret = _clientSecret
        };
        
        var scopes = new[]
        {
            SheetsService.Scope.Drive, SheetsService.Scope.DriveFile, SheetsService.Scope.Spreadsheets
        };
        
        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync
            (clientSecrets, scopes, _username, CancellationToken.None, new FileDataStore(_applicationName));

        var service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = _applicationName
        });

        return service;
    }
    
    
}
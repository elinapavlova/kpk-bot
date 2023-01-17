using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Helpers;

public static class GoogleDriveQueryBuilder
{
    public static string Build(Dictionary<GoogleDriveQueryParameterType, KeyValuePair<string, string>> parameters)
    {
        var queries = parameters.Select(x => x.Key switch
            {
                GoogleDriveQueryParameterType.Contains => $"{x.Value.Key} contains '{x.Value.Value}'",
                GoogleDriveQueryParameterType.Equals => $"{x.Value.Key}='{x.Value.Value}'",
                GoogleDriveQueryParameterType.NotEquals => $"{x.Value.Key}!='{x.Value.Value}'",
                GoogleDriveQueryParameterType.In => $"'{x.Value.Value}' in {x.Value.Key}",
                _ => throw new Exception($"Необрабатываемый тип параметра {x.Key}")
            })
            .ToList();

        var query = string.Empty;
        
        foreach (var x in queries)
        {
            if (queries.Last() == x)
            {
                query += x;
                break;
            }
            query += $"{x} and ";
        }

        return query;
    }
}
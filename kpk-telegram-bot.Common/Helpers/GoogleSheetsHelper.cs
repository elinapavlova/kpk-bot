using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace kpk_telegram_bot.Common.Helpers;

public static class GoogleSheetsHelper
{
    public static string GoogleSheetsUpdate(string fileId, string inputOption, string inputRange, string inputText, SheetsService sheetsService) 
    {
        var result = "Success";
        try 
        {
            var valueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            inputOption = inputOption.ToLower();
            if (inputOption == "raw") 
            {
                valueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            }

            var oRow = inputText.Split('|');
            var oBody = new ValueRange 
            {
                Values = new List<IList<object>> { oRow }
            };
            
            var oRequest = sheetsService.Spreadsheets.Values.Update(oBody, fileId, inputRange);
            oRequest.ValueInputOption = valueInputOption;
            var oResponse = oRequest.Execute();
        } 
        catch (Exception e) 
        {
            result = "Google Sheets updating error: " + e.Message;
        }
        return result;
    }
}
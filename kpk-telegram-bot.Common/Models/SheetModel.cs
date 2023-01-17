namespace kpk_telegram_bot.Common.Models;

 public class SheetModel
{
    public string SpreadsheetId { get; set; }
    public List<ValueRange> ValueRanges { get; set; }
}

public class ValueRange
{
    public string Range { get; set; }
    public string MajorDimension { get; set; }
    public List<List<string>> Values { get; set; }
}
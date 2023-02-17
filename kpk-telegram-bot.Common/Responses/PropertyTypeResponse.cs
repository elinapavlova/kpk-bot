namespace kpk_telegram_bot.Common.Responses;

public record PropertyTypeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
}
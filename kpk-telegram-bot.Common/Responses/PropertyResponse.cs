namespace kpk_telegram_bot.Common.Responses;

public record PropertyResponse
{
    public Guid Id { get; set; }
    public string Value { get; set; }
    public PropertyTypeResponse Type { get; set; }
}
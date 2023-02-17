namespace kpk_telegram_bot.Common.Models;

public record ItemCreateModel(Guid TypeId, List<ItemPropertyCreateModel> Properties, Guid? ParentId = null)
{
    public Guid TypeId { get; set; } = TypeId;
    public Guid? ParentId { get; set; } = ParentId;
    public List<ItemPropertyCreateModel> Properties { get; set; } = Properties;
}

public record ItemPropertyCreateModel(Guid TypeId, string? Value)
{
    public Guid TypeId { get; set; } = TypeId;
    public string? Value { get; set; } = Value;
}
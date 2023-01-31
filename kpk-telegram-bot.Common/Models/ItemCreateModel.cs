namespace kpk_telegram_bot.Common.Models;

public class ItemCreateModel
{
    public Guid TypeId { get; set; }
    public Guid? ParentId { get; set; }
    public List<ItemPropertyCreateModel> Properties { get; set; }
}

public class ItemPropertyCreateModel
{
    public Guid TypeId { get; set; }
    public string? Value { get; set; }

    public ItemPropertyCreateModel(Guid typeId, string? value)
    {
        TypeId = typeId;
        Value = value;
    }
}
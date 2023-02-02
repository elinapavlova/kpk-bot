namespace kpk_telegram_bot.Common.Responses;

public class ItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public Guid? ParentId { get; set; }
    public List<ItemResponse>? Items { get; set; }
    public List<PropertyResponse> Properties { get; set; }

}
namespace kpk_telegram_bot.Common.Database.Base;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime? DateUpdated { get; set; }
    public DateTime? DateDeleted { get; set; }
}
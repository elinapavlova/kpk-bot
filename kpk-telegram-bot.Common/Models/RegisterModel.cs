namespace kpk_telegram_bot.Common.Models;

public class RegisterModel
{
    public long Id { get; set; }
    public string[] Info { get; set; }

    public RegisterModel(long id, string[] info)
    {
        Id = id;
        Info = info;
    }
}
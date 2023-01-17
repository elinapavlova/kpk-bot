using kpk_telegram_bot.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Common.Database;

public class KpkTelegramBotContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<GroupEntity> Groups { get; set; }

    public KpkTelegramBotContext(DbContextOptions<KpkTelegramBotContext> options) : base(options)         
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserEntity>(x =>
        {
            x.Property(user => user.UserName).IsRequired();
            x.Property(user => user.RoleId).IsRequired();
            
            x.HasOne(user => user.Group)
                .WithMany(group => group.Users)
                .HasForeignKey(user => user.GroupId);
        });

        builder.Entity<GroupEntity>(x =>
        {
            x.Property(group => group.Name).IsRequired();
        });
    }
}
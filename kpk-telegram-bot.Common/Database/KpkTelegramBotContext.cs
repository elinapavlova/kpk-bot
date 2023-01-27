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

        builder.Entity<ItemPropertyTypeEntity>(x =>
        {
            x.Property(prop => prop.Name).IsRequired().HasMaxLength(300);
            x.Property(prop => prop.Value).IsRequired().HasMaxLength(300);
        });        
        
        builder.Entity<ItemPropertyEntity>(x =>
        {
            x.Property(prop => prop.Value).IsRequired().HasMaxLength(300);

            x.HasOne(prop => prop.Type)
                .WithMany(type => type.Properties)
                .HasForeignKey(prop => prop.TypeId);            
            
            x.HasOne(prop => prop.Item)
                .WithMany(item => item.Properties)
                .HasForeignKey(prop => prop.ItemId);
        });

        builder.Entity<ItemTypeEntity>(x =>
        {
            x.Property(item => item.Name).IsRequired().HasMaxLength(300);
            x.Property(item => item.Value).IsRequired().HasMaxLength(300);
        });        
        
        builder.Entity<ItemEntity>(x =>
        {
            x.HasOne(prop => prop.Type)
                .WithMany(type => type.Items)
                .HasForeignKey(prop => prop.TypeId);  
        });

        builder.Entity<UserEntity>(x =>
        {
            x.Property(user => user.UserName).IsRequired();
            x.Property(user => user.RoleId).IsRequired();
            
            x.HasOne(user => user.Group)
                .WithMany(group => group.Users)
                .HasForeignKey(user => user.GroupId);
        });
    }
}
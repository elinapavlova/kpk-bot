using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Common.Database.Base;

public interface IBaseRepository<TModel, TKey>
    where TModel : BaseEntity<TKey>
{
    Task<TransactionContainer?> BeginTransaction();
}

public abstract class BaseRepository<TModel, TKey> : IBaseRepository<TModel, TKey>
    where TModel : BaseEntity<TKey>
{
    private readonly KpkTelegramBotContext _context;

    protected BaseRepository(KpkTelegramBotContext context)
    {
        _context = context;
    }

    protected async Task Execute(Func<DbSet<TModel>, Task> func)
    {
        var dbSet = _context.Set<TModel>();
        await func(dbSet);
        await _context.SaveChangesAsync();
    }

    protected async Task<TResult> ExecuteWithResult<TResult>(Func<DbSet<TModel>, Task<TResult>> func)
    {
        var dbSet = _context.Set<TModel>();
        var result = await func(dbSet);
        await _context.SaveChangesAsync();
        return result;
    }
    
    protected async Task<TResult> ExecuteWithoutSavingResult<TResult>(Func<DbSet<TModel>, Task<TResult>> func)
    {
        var dbSet = _context.Set<TModel>();
        var result = await func(dbSet);
        return result;
    }

    public async Task<TransactionContainer?> BeginTransaction()
    {
        var contextTransaction = await _context.Database.BeginTransactionAsync();
        return new TransactionContainer(contextTransaction, false);
    }
}
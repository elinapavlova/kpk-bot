using Microsoft.EntityFrameworkCore.Storage;

namespace kpk_telegram_bot.Common.Database;

public class TransactionContainer : IAsyncDisposable
{
    public bool HasError { get; set; }
    public IDbContextTransaction ContextTransaction { get; set; }

    public TransactionContainer(IDbContextTransaction contextTransaction, bool hasError)
    {
        HasError = hasError;
        ContextTransaction = contextTransaction;
    }
        
    public async ValueTask DisposeAsync()
    {
        if (HasError is false)
        {
            await ContextTransaction.CommitAsync();
            return;
        }
        await ContextTransaction.RollbackAsync();
    }
}
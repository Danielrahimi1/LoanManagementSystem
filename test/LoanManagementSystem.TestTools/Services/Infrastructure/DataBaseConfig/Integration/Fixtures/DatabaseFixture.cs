using System.Transactions;

namespace LoanManagementSystem.TestTools.Services.Infrastructure.DataBaseConfig.Integration.Fixtures;

public class DatabaseFixture : IDisposable
{
    private readonly TransactionScope _transactionScope;

    public DatabaseFixture()
    {
        _transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            TransactionScopeAsyncFlowOption.Enabled);
    }

    public virtual void Dispose()
    {
        _transactionScope?.Dispose();
    }
}
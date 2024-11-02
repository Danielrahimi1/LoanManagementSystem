using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Persistence.Ef.UnitOfWorks;

public class EfUnitOfWork(EfDataContext context) : UnitOfWork
{
    public async Task Save() => await context.SaveChangesAsync();

    public async Task Begin() => await context.Database.BeginTransactionAsync();

    public async Task Rollback() => await context.Database.RollbackTransactionAsync();

    public async Task Commit()
    {
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
    }
}
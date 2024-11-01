using System.Threading.Tasks;

namespace LoanManagementSystem.Services.UnitOfWorks;

public interface UnitOfWork
{
    Task Save();
    Task Begin();
    Task Rollback();
    Task Commit();
}
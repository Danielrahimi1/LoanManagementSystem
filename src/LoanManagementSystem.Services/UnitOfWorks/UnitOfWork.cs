using System.Threading.Tasks;
using LoanManagementSystem.Contracts.Interfaces;

namespace LoanManagementSystem.Services.UnitOfWorks;

public interface UnitOfWork : IScope
{
    Task Save();
    Task Begin();
    Task Rollback();
    Task Commit();
}
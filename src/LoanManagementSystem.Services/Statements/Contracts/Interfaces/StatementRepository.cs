using System.Threading.Tasks;
using LoanManagementSystem.Entities.Statements;

namespace LoanManagementSystem.Services.Statements.Contracts.Interfaces;

public interface StatementRepository
{
    Task Add(Statement statement);
    Task<Statement?> Find(int id);
    Task Update(Statement statement);
}
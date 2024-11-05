using System.Threading.Tasks;
using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.Services.Statements.Contracts.Interfaces;

public interface StatementRepository
{
    Task Add(Statement statement);
    Task<Statement?> Find(int id);
    void Update(Statement statement);
}
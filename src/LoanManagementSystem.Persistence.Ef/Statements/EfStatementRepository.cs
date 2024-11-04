using LoanManagementSystem.Entities.Statements;
using LoanManagementSystem.Services.Statements.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.Statements;

public class EfStatementRepository(EfDataContext context) : StatementRepository
{
    public async Task Add(Statement statement) =>
        await context.Set<Statement>().AddAsync(statement);

    public async Task<Statement?> Find(int id) =>
        await context.Set<Statement>().FirstOrDefaultAsync(s => s.Id == id);

    public void Update(Statement statement) =>
        context.Set<Statement>().Update(statement);
}
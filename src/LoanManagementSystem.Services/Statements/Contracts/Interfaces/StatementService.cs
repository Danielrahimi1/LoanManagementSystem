using System.Threading.Tasks;
using LoanManagementSystem.Services.Statements.Contracts.DTOs;

namespace LoanManagementSystem.Services.Statements.Contracts.Interfaces;

public interface StatementService
{
    Task Add(AddStatementDto dto);
    Task Update(UpdateStatementDto dto);
}
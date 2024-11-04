using System.Threading.Tasks;

namespace LoanManagementSystem.Services.Installments.Contracts.Interfaces;

public interface InstallmentService
{
    Task Pay(int id);
}
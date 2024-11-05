using System.Threading.Tasks;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.Services.Installments.Exceptions;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.Installments;

public class InstallmentAppService(
    InstallmentRepository installmentRepository,
    UnitOfWork unitOfWork,
    DateService dateService) : InstallmentService
{
    public async Task Pay(int id)
    {
        var installment = await installmentRepository.Find(id);
        if (installment is null)
        {
            throw new InstallmentNotFoundException();
        }

        if (installment.PaymentDate is not null)
        {
            throw new InstallmentAlreadyPaidException();
        }

        installment.PaymentDate = dateService.UtcNow;
        if (installment.PaymentDeadLine < installment.PaymentDate)
        {
            installment.Fine = installment.Amount * 0.05M;
        }
        installmentRepository.Update(installment);
        await unitOfWork.Save();
    }
}
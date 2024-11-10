using System;
using System.Threading.Tasks;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.Services.Installments.Exceptions;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.Installments;

public class InstallmentAppService(
    InstallmentRepository installmentRepository,
    UnitOfWork unitOfWork
) : InstallmentService
{
    public async Task<decimal> Pay(int loanRequestId)
    {
        var installment = await installmentRepository.GetFirstUnpaidInstallment(loanRequestId);
        if (installment is null)
        {
            throw new InstallmentNotFoundException();
        }

        installment.PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (installment.PaymentDeadLine < installment.PaymentDate)
        {
            installment.Fine = installment.Amount * 0.02M;
        }

        installmentRepository.Update(installment);
        await unitOfWork.Save();
        return installment.Amount + installment.MonthlyInterest + installment.Fine;
    }
}
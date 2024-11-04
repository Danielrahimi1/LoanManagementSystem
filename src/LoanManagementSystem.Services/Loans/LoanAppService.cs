using System.Threading.Tasks;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using LoanManagementSystem.Services.Loans.Exceptions;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.Loans;

public class LoanAppService(
    LoanRepository loanRepository,
    UnitOfWork unitOfWork) : LoanService
{
    public async Task Create(AddLoanDto dto)
    {
        var loan = new Loan
        {
            Amount = dto.Amount,
            InstallmentCount = dto.InstallmentCount,
            AnnualInterestRate = dto.AnnualInterestRate
        };
        await loanRepository.Add(loan);
        await unitOfWork.Save();
    }

    public async Task Update(int id, UpdateLoanDto dto)
    {
        var loan = await loanRepository.Find(id);
        if (loan is null)
        {
            throw new LoanNotFoundException();
        }

        loan.InstallmentCount = dto.InstallmentCount ?? loan.InstallmentCount;
        loan.AnnualInterestRate = dto.AnnualInterestRate ?? loan.AnnualInterestRate;
        loan.Amount = dto.Amount ?? loan.Amount;
        loanRepository.Update(loan);
        await unitOfWork.Save();
    }

    public async Task Delete(int id)
    {
        var loan = await loanRepository.Find(id);
        if (loan is null)
        {
            throw new LoanNotFoundException();
        }

        loanRepository.Remove(loan);
        await unitOfWork.Save();
    }
}
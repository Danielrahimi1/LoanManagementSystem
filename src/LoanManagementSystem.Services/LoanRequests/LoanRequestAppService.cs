using System;
using System.Linq;
using System.Threading.Tasks;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Exceptions;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using LoanManagementSystem.Services.Loans.Exceptions;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.LoanRequests;

public class LoanRequestAppService(
    LoanRequestRepository loanRequestRepository,
    CustomerRepository customerRepository,
    LoanRepository loanRepository,
    InstallmentRepository installmentRepository,
    DateService dateService,
    UnitOfWork unitOfWork)
    : LoanRequestService
{
    public async Task Open(int customerId, AddLoanRequestDto dto)
    {
        var customer = await customerRepository.Find(customerId);
        var loan = await loanRepository.Find(dto.LoanId);
        if (customer is null)
        {
            throw new CustomerNotFoundException();
        }

        if (!customer.IsVerified)
        {
            throw new CustomerNotVerifiedException();
        }

        if (await loanRequestRepository.HasActiveLoanRequests(customerId))
        {
            throw new CustomerHasActiveLoanRequestsException();
        }

        if (loan is null)
        {
            throw new LoanNotFoundException();
        }

        var onTimeClosedLoans = await loanRequestRepository.CountNonDelayedLoans(customerId) * 30;
        var delayedInstallments = await installmentRepository.CountDelayedInstallments(customerId) * -5;
        var loanNetWorthRatio = customer.NetWorth == 0 ? 0 :
            loan.Amount < customer.NetWorth * 0.5M ? 20 :
            loan.Amount < customer.NetWorth * 0.7M ? 10 : 0;

        var rate = (int)customer.IncomeGroup +
                   (int)customer.JobType +
                   onTimeClosedLoans +
                   loanNetWorthRatio +
                   delayedInstallments;
        if (rate < 60)
        {
            throw new RateBelowSixtyException();
        }

        var lr = new LoanRequest
        {
            LoanId = dto.LoanId,
            CustomerId = dto.CustomerId,
            Status = LoanRequestStatus.Review,
            DelayInRepayment = false,
            Rate = rate > 100 ? 100 : rate,
            ConfirmationDate = dateService.UtcNow,
            Customer = customer,
        };

        await loanRequestRepository.Add(lr);
        await unitOfWork.Save();
    }

    public async Task Accept(int id)
    {
        var lr = await loanRequestRepository.Find(id);
        if (lr is null)
        {
            throw new LoanRequestNotFoundException();
        }

        if (lr.Status != LoanRequestStatus.Review)
        {
            throw new LoanRequestMustBeReviewedException();
        }

        lr.Status = LoanRequestStatus.Accept;

        loanRequestRepository.Update(lr);
        await unitOfWork.Save();
    }

    public async Task Reject(int id)
    {
        var lr = await loanRequestRepository.Find(id);
        if (lr is null)
        {
            throw new LoanRequestNotFoundException();
        }

        if (lr.Status != LoanRequestStatus.Review)
        {
            throw new LoanRequestMustBeReviewedException();
        }

        lr.Status = LoanRequestStatus.Reject;

        loanRequestRepository.Update(lr);
        await unitOfWork.Save();
    }

    public async Task Activate(int id)
    {
        // TODO: Change lr state to Activate, after first installment paid.
        
        var lr = await loanRequestRepository.Find(id);
        if (lr is null)
        {
            throw new LoanRequestNotFoundException();
        }

        var loan = await loanRepository.Find(lr.LoanId);

        if (lr.Status != LoanRequestStatus.Accept)
        {
            throw new LoanRequestMustBeAcceptedException();
        }

        lr.Status = LoanRequestStatus.Active;
        lr.ConfirmationDate = dateService.UtcNow;
        var installmentAmount = loan!.Amount / loan.InstallmentCount;
        var monthlyInterest = installmentAmount * (loan.AnnualInterestRate / 12M);
        lr.Installments.UnionWith(
            from month in Enumerable.Range(1, loan.InstallmentCount + 1)
            select new Installment
            {
                LoanRequestId = lr.Id,
                LoanRequest = lr,
                Amount = installmentAmount,
                MonthlyInterest = monthlyInterest,
                PaymentDeadLine = lr.ConfirmationDate.Value.AddMonths(month),
                Fine = 0,
            }
        );

        loanRequestRepository.Update(lr);
        await unitOfWork.Save();
    }

    public async Task Pay(int id)
    {
        // var installment = await installmentRepository.Find(id);
        // if (installment is null)
        // {
        //     throw new InstallmentNotFoundException();
        // }
        //
        // if (installment.PaymentDate is not null)
        // {
        //     throw new InstallmentAlreadyPaidException();
        // }
        //
        // installment.PaymentDate = dateService.UtcNow;
        // if (installment.PaymentDeadLine < installment.PaymentDate)
        // {
        //     installment.Fine = installment.Amount * 0.05M;
        // }
        // installmentRepository.Update(installment);
        // await unitOfWork.Save();
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
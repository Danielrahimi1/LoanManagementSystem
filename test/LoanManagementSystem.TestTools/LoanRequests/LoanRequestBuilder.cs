using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Entities.LoanRequests.Enums;

namespace LoanManagementSystem.TestTools.LoanRequests;

public class LoanRequestBuilder
{
    private readonly LoanRequest _loanRequest;

    public LoanRequestBuilder()
    {
        _loanRequest = new LoanRequest
        {
            LoanId = 1,
            Status = (LoanRequestStatus)0,
            DelayInRepayment = false,
            ConfirmationDate = null,
            Installments = []
        };
    }
    
    public LoanRequestBuilder WithCustomer(Customer value)
    {
        _loanRequest.Customer = value;
        return this;
    }

    public LoanRequestBuilder WithLoanId(int value)
    {
        _loanRequest.LoanId = value;
        return this;
    }

    public LoanRequestBuilder WithStatus(LoanRequestStatus value)
    {
        _loanRequest.Status = value;
        return this;
    }
    public LoanRequestBuilder WithDelayInRepayment(bool value)
    {
        _loanRequest.DelayInRepayment = value;
        return this;
    }
    public LoanRequestBuilder WithConfirmationDate(DateOnly value)
    {
        _loanRequest.ConfirmationDate = value;
        return this;
    }
    public LoanRequestBuilder WithInstallments(params Installment[] installments)
    {
        _loanRequest.Installments.UnionWith(installments);
        return this;
    }
    
    public LoanRequest Build()
    {
        return _loanRequest;
    }
}
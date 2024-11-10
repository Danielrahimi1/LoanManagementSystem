using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests;

namespace LoanManagementSystem.TestTools.Services.Installments;

public class InstallmentBuilder
{
    private readonly Installment _installment;

    public InstallmentBuilder()
    {
        _installment = new Installment
        {
            Amount = 0,
            MonthlyInterest = 0,
            PaymentDeadLine = default,
            PaymentDate = null,
            Fine = 0
        };
    }

    public InstallmentBuilder WithLoanRequest(LoanRequest value)
    {
        _installment.LoanRequest = value;
        return this;
    }

    public InstallmentBuilder WithAmount(decimal value)
    {
        _installment.Amount = value;
        return this;
    }

    public InstallmentBuilder WithMonthlyInterest(decimal value)
    {
        _installment.MonthlyInterest = value;
        return this;
    }

    public InstallmentBuilder WithPaymentDeadLine(DateOnly value)
    {
        _installment.PaymentDeadLine = value;
        return this;
    }

    public InstallmentBuilder WithPaymentDate(DateOnly value)
    {
        _installment.PaymentDate = value;
        return this;
    }

    public InstallmentBuilder WithFine(decimal value)
    {
        _installment.Fine = value;
        return this;
    }

    public Installment Build() => _installment;
}
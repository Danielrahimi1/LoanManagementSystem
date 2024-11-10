using LoanManagementSystem.Entities.Loans;

namespace LoanManagementSystem.TestTools.Services.Loans;

public class LoanBuilder
{
    private readonly Loan _loan;

    public LoanBuilder()
    {
        _loan = new Loan
        {
            Amount = 100,
            InstallmentCount = 10,
            AnnualInterestRate = 15
        };
    }

    public LoanBuilder WithAmount(decimal value)
    {
        _loan.Amount = value;
        return this;
    }

    public LoanBuilder WithInstallmentCount(int value)
    {
        _loan.InstallmentCount = value;
        return this;
    }

    public LoanBuilder WithAnnualInterestRate(int value)
    {
        _loan.AnnualInterestRate = value;
        return this;
    }

    public Loan Build() => _loan;
}
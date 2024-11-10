using LoanManagementSystem.Services.Loans.Contracts.DTOs;

namespace LoanManagementSystem.TestTools.Services.Loans;

public class AddLoanDtoBuilder
{
    private readonly AddLoanDto _loan;

    public AddLoanDtoBuilder()
    {
        _loan = new AddLoanDto()
        {
            Amount = 12,
            InstallmentCount = 10,
        };
    }

    public AddLoanDtoBuilder WithAmount(decimal value)
    {
        _loan.Amount = value;
        return this;
    }

    public AddLoanDtoBuilder WithInstallmentCount(int value)
    {
        _loan.InstallmentCount = value;
        return this;
    }

    public AddLoanDto Build() => _loan;
    
}
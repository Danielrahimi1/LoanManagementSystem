using FluentAssertions;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using LoanManagementSystem.Services.Loans.Exceptions;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Loans;

namespace LoanManagementSystem.Services.Tests.Unit.Loans;

public class LoanServiceTests : BusinessIntegrationTest
{
    private readonly LoanService _sut;

    public LoanServiceTests()
    {
        _sut = LoanServiceFactory.CreateService(SetupContext);
    }

    [Fact]
    public async Task Create_add_loan_when_given_AddLoanDto()
    {
        var dto = new AddLoanDto
        {
            Amount = 10,
            InstallmentCount = 12,
            AnnualInterestRate = 18
        };

        await _sut.Create(dto);

        var expected = ReadContext.Set<Loan>().Single();
        expected.Amount.Should().Be(dto.Amount);
        expected.InstallmentCount.Should().Be(dto.InstallmentCount);
        expected.AnnualInterestRate.Should().Be(dto.AnnualInterestRate);
    }

    [Fact]
    public async Task Update_update_loan_when_given_UpdateLoanDto()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);

        var dto = new UpdateLoanDto
        {
            Amount = 10,
            InstallmentCount = 12,
            AnnualInterestRate = 18
        };

        await _sut.Update(loan.Id, dto);

        var expected = ReadContext.Set<Loan>().Single();
        expected.Amount.Should().Be(dto.Amount);
        expected.InstallmentCount.Should().Be(dto.InstallmentCount);
        expected.AnnualInterestRate.Should().Be(dto.AnnualInterestRate);
    }

    [Fact]
    public async Task Update_throws_exception_when_loanId_not_found()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);

        var dto = new UpdateLoanDto
        {
            Amount = 10,
            InstallmentCount = 12,
            AnnualInterestRate = 18
        };

        var expected = async () => await _sut.Update(-1, dto);

        await expected.Should().ThrowExactlyAsync<LoanNotFoundException>();
    }

    [Fact]
    public async Task Delete_remove_loan_when_given_loanId()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var loan2 = new LoanBuilder().WithAmount(100M).Build();
        Save(loan2);

        await _sut.Delete(loan.Id);

        var expected = ReadContext.Set<Loan>().Single();
        expected.Id.Should().Be(loan2.Id);
    }

    [Fact]
    public async Task Delete_throw_exception_when_loanId_not_found()
    {
        var expected = async () => await _sut.Delete(-1);

        await expected.Should().ThrowExactlyAsync<LoanNotFoundException>();
    }
}
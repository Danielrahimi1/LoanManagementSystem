using FluentAssertions;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Loans;

namespace LoanManagementSystem.Services.Tests.Unit.Loans;

public class LoanQueryTests : BusinessIntegrationTest
{
    private readonly LoanQuery _sut;

    public LoanQueryTests()
    {
        _sut = new EfLoanQuery(ReadContext);
    }

    [Fact]
    public async Task GetById_return_GetLoanDto__when_passed_loan_id()
    {
        var loan1 = new LoanBuilder().WithAmount(100)
            .WithInstallmentCount(60).WithAnnualInterestRate(4).Build();
        var loan2 = new LoanBuilder().WithAmount(200)
            .WithInstallmentCount(60).WithAnnualInterestRate(4).Build();
        var loan3 = new LoanBuilder().WithAmount(300)
            .WithInstallmentCount(48).WithAnnualInterestRate(2).Build();
        Save(loan1, loan2, loan3);

        var actual = await _sut.GetById(loan3.Id);

        actual!.Amount.Should().Be(loan3.Amount);
        actual.InstallmentCount.Should().Be(loan3.InstallmentCount);
        actual.AnnualInterestRate.Should().Be(loan3.AnnualInterestRate);
    }


    [Fact]
    public async Task GetById_return_null_GetLoanDto__when_passed_loan_id_not_found()
    {
        var loan1 = new LoanBuilder().WithAmount(100)
            .WithInstallmentCount(60).WithAnnualInterestRate(4).Build();
        Save(loan1);
        var actual = await _sut.GetById(-1);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_return_all_when_invoked()
    {
        var loan1 = new LoanBuilder().WithAmount(100)
            .WithInstallmentCount(60).WithAnnualInterestRate(4).Build();
        var loan2 = new LoanBuilder().WithAmount(200)
            .WithInstallmentCount(60).WithAnnualInterestRate(4).Build();
        Save(loan1, loan2);

        var actual = await _sut.GetAll();

        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanDto
            {
                Amount = loan1.Amount,
                InstallmentCount = loan1.InstallmentCount,
                AnnualInterestRate = loan1.AnnualInterestRate
            },
            new GetLoanDto
            {
                Amount = loan2.Amount,
                InstallmentCount = loan2.InstallmentCount,
                AnnualInterestRate = loan2.AnnualInterestRate
            }
        ]);
    }

    [Fact]
    public async Task GetAllShortPeriod_return_all_short_period_loans_when_invoked()
    {
        var loan1 = new LoanBuilder().WithAmount(100)
            .WithInstallmentCount(9).WithAnnualInterestRate(4).Build();
        var loan2 = new LoanBuilder().WithAmount(200)
            .WithInstallmentCount(12).WithAnnualInterestRate(4).Build();
        var loan3 = new LoanBuilder().WithAmount(100)
            .WithInstallmentCount(60).WithAnnualInterestRate(4).Build();
        Save(loan1, loan2, loan3);

        var actual = await _sut.GetAllShortPeriod();

        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanDto
            {
                Amount = loan1.Amount,
                InstallmentCount = loan1.InstallmentCount,
                AnnualInterestRate = loan1.AnnualInterestRate
            },
            new GetLoanDto
            {
                Amount = loan2.Amount,
                InstallmentCount = loan2.InstallmentCount,
                AnnualInterestRate = loan2.AnnualInterestRate
            }
        ]);
    }
    
    [Fact]
    public async Task GetAllLongPeriod_return_all_long_period_loans_when_invoked()
    {
        var loan1 = new LoanBuilder().WithAmount(100)
            .WithInstallmentCount(9).WithAnnualInterestRate(4).Build();
        var loan2 = new LoanBuilder().WithAmount(200)
            .WithInstallmentCount(24).WithAnnualInterestRate(4).Build();
        var loan3 = new LoanBuilder().WithAmount(100)
            .WithInstallmentCount(60).WithAnnualInterestRate(4).Build();
        Save(loan1, loan2, loan3);

        var actual = await _sut.GetAllLongPeriod();

        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanDto
            {
                Amount = loan3.Amount,
                InstallmentCount = loan3.InstallmentCount,
                AnnualInterestRate = loan3.AnnualInterestRate
            },
            new GetLoanDto
            {
                Amount = loan2.Amount,
                InstallmentCount = loan2.InstallmentCount,
                AnnualInterestRate = loan2.AnnualInterestRate
            }
        ]);
    }
    
}
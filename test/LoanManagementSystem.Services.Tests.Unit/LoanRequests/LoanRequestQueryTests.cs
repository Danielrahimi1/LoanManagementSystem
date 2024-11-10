using FluentAssertions;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Persistence.Ef.LoanRequests;
using LoanManagementSystem.Services.Installments.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.TestTools.Services.Customers;
using LoanManagementSystem.TestTools.Services.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Services.Installments;
using LoanManagementSystem.TestTools.Services.LoanRequests;
using LoanManagementSystem.TestTools.Services.Loans;

namespace LoanManagementSystem.Services.Tests.Unit.LoanRequests;

public class LoanRequestQueryTests : BusinessIntegrationTest
{
    private readonly LoanRequestQuery _sut;

    public LoanRequestQueryTests() =>
        _sut = new EfLoanRequestQuery(ReadContext);

    [Fact]
    public async Task GetById_return_GetLoanRequestDto_when_passed_loanRequest_id()
    {
        var c = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder().Build();
        var lr2 = new LoanRequestBuilder().Build();
        c.LoanRequests.UnionWith([lr1, lr2]);
        Save(c);

        var actual = await _sut.GetById(lr1.Id);

        actual!.LoanId.Should().Be(lr1.LoanId);
        actual.CustomerId.Should().Be(lr1.CustomerId);
        actual.Status.Should().Be(lr1.Status.ToString());
        actual.DelayInRepayment.Should().Be(lr1.DelayInRepayment);
        actual.Rate.Should().Be(lr1.Rate);
        actual.ConfirmationDate.Should().Be(lr1.ConfirmationDate);
    }

    [Fact]
    public async Task GetAllByCustomer_return_specific_customer_loanRequests_when_given_customer_id()
    {
        var loan1 = new LoanBuilder().Build();
        Save(loan1);
        var customer1 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder()
            .WithLoanId(loan1.Id).WithRate(15)
            .WithStatus(LoanRequestStatus.Close).WithDelayInRepayment(false)
            .WithConfirmationDate(new DateOnly(2020, 1, 1))
            .Build();
        var lr2 = new LoanRequestBuilder()
            .WithLoanId(loan1.Id).WithRate(20)
            .WithStatus(LoanRequestStatus.Reject).WithDelayInRepayment(false)
            .WithConfirmationDate(new DateOnly(2019, 12, 30))
            .Build();
        customer1.LoanRequests.UnionWith([lr1, lr2]);
        var customer2 = new CustomerBuilder().Build();
        customer2.LoanRequests.UnionWith([
            new LoanRequestBuilder().Build(),
        ]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllByCustomer(customer1.Id);

        actual.Should().BeEquivalentTo([
            new GetLoanRequestDto
            {
                LoanId = lr1.LoanId,
                CustomerId = lr1.CustomerId,
                Rate = lr1.Rate,
                Status = lr1.Status.ToString(),
                DelayInRepayment = lr1.DelayInRepayment,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestDto
            {
                LoanId = lr2.LoanId,
                CustomerId = lr2.CustomerId,
                Rate = lr2.Rate,
                Status = lr2.Status.ToString(),
                DelayInRepayment = lr2.DelayInRepayment,
                ConfirmationDate = lr2.ConfirmationDate
            }
        ]);
    }

    [Fact]
    public async Task GetAll_return_all_loanRequests_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        var customer2 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder().WithLoanId(loan.Id).Build();
        var lr2 = new LoanRequestBuilder().WithLoanId(loan.Id).Build();
        var lr3 = new LoanRequestBuilder().WithLoanId(loan.Id).Build();
        customer1.LoanRequests.UnionWith([lr1]);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAll();

        actual.Should().BeEquivalentTo([
            new GetLoanRequestDto
            {
                LoanId = lr1.LoanId,
                CustomerId = lr1.CustomerId,
                Rate = lr1.Rate,
                Status = lr1.Status.ToString(),
                DelayInRepayment = lr1.DelayInRepayment,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestDto
            {
                LoanId = lr2.LoanId,
                CustomerId = lr2.CustomerId,
                Rate = lr2.Rate,
                Status = lr2.Status.ToString(),
                DelayInRepayment = lr2.DelayInRepayment,
                ConfirmationDate = lr2.ConfirmationDate
            },
            new GetLoanRequestDto
            {
                LoanId = lr3.LoanId,
                CustomerId = lr3.CustomerId,
                Rate = lr3.Rate,
                Status = lr3.Status.ToString(),
                DelayInRepayment = lr3.DelayInRepayment,
                ConfirmationDate = lr3.ConfirmationDate
            },
        ]);
    }

    [Fact]
    public async Task GetAllAcceptLoans_return_all_accept_loans_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        var customer2 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Accept).WithLoanId(loan.Id).Build();
        var lr2 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Reject).WithLoanId(loan.Id).Build();
        var lr3 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Accept).WithLoanId(loan.Id).Build();
        customer1.LoanRequests.UnionWith([lr1]);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllAcceptLoans();

        // actual.Should().Satisfy(lr => lr.Status == LoanRequestStatus.Active.ToString());
        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanRequestDto
            {
                LoanId = lr1.LoanId,
                CustomerId = lr1.CustomerId,
                Rate = lr1.Rate,
                Status = LoanRequestStatus.Accept.ToString(),
                DelayInRepayment = lr1.DelayInRepayment,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestDto
            {
                LoanId = lr3.LoanId,
                CustomerId = lr3.CustomerId,
                Rate = lr3.Rate,
                Status = LoanRequestStatus.Accept.ToString(),
                DelayInRepayment = lr3.DelayInRepayment,
                ConfirmationDate = lr3.ConfirmationDate
            },
        ]);
    }

    [Fact]
    public async Task GetAllActiveLoans_return_all_active_loans_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        var customer2 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        var lr2 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Reject).WithLoanId(loan.Id).Build();
        var lr3 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        customer1.LoanRequests.UnionWith([lr1]);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllActiveLoans();

        // actual.Should().Satisfy(lr => lr.Status == LoanRequestStatus.Active.ToString());
        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanRequestDto
            {
                LoanId = lr1.LoanId,
                CustomerId = lr1.CustomerId,
                Rate = lr1.Rate,
                Status = LoanRequestStatus.Active.ToString(),
                DelayInRepayment = lr1.DelayInRepayment,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestDto
            {
                LoanId = lr3.LoanId,
                CustomerId = lr3.CustomerId,
                Rate = lr3.Rate,
                Status = LoanRequestStatus.Active.ToString(),
                DelayInRepayment = lr3.DelayInRepayment,
                ConfirmationDate = lr3.ConfirmationDate
            },
        ]);
    }

    [Fact]
    public async Task GetAllDelayedLoans_return_all_delayed_loans_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        var customer2 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder()
            .WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        var lr2 = new LoanRequestBuilder()
            .WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        var lr3 = new LoanRequestBuilder()
            .WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        customer1.LoanRequests.UnionWith([lr1]);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllDelayedLoans();

        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanRequestDto
            {
                LoanId = lr1.LoanId,
                CustomerId = lr1.CustomerId,
                Rate = lr1.Rate,
                Status = lr1.Status.ToString(),
                DelayInRepayment = true,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestDto
            {
                LoanId = lr3.LoanId,
                CustomerId = lr3.CustomerId,
                Rate = lr3.Rate,
                Status = lr3.Status.ToString(),
                DelayInRepayment = true,
                ConfirmationDate = lr3.ConfirmationDate
            },
        ]);
    }

    [Fact]
    public async Task GetAllCloseLoans_return_all_done_loans_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        var customer2 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Close).WithLoanId(loan.Id).Build();
        var lr2 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Close).WithLoanId(loan.Id).Build();
        var lr3 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        customer1.LoanRequests.UnionWith([lr1]);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllClosedLoans();

        // actual.Should().Satisfy(lr => lr.Status == LoanRequestStatus.Active.ToString());
        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanRequestDto
            {
                LoanId = lr1.LoanId,
                CustomerId = lr1.CustomerId,
                Rate = lr1.Rate,
                Status = LoanRequestStatus.Close.ToString(),
                DelayInRepayment = lr1.DelayInRepayment,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestDto
            {
                LoanId = lr2.LoanId,
                CustomerId = lr2.CustomerId,
                Rate = lr2.Rate,
                Status = LoanRequestStatus.Close.ToString(),
                DelayInRepayment = lr2.DelayInRepayment,
                ConfirmationDate = lr2.ConfirmationDate
            },
        ]);
    }

    [Fact]
    public async Task GetAllWithCustomer_return_all_loanRequests_with_customer_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        var customer2 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder().WithLoanId(loan.Id).Build();
        var lr2 = new LoanRequestBuilder().WithLoanId(loan.Id).Build();
        var lr3 = new LoanRequestBuilder().WithLoanId(loan.Id).Build();
        customer1.LoanRequests.UnionWith([lr1]);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllWithCustomer();

        actual.Should().BeEquivalentTo([
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer1.FirstName,
                LastName = customer1.LastName,
                NationalId = customer1.NationalId,
                PhoneNumber = customer1.PhoneNumber,
                Email = customer1.Email,
                LoanId = lr1.LoanId,
                Rate = lr1.Rate,
                Status = lr1.Status.ToString(),
                DelayInRepayment = lr1.DelayInRepayment,
                ConfirmationDate = lr1.ConfirmationDate,
            },
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer2.FirstName,
                LastName = customer2.LastName,
                NationalId = customer2.NationalId,
                PhoneNumber = customer2.PhoneNumber,
                Email = customer2.Email,
                LoanId = lr2.LoanId,
                Rate = lr2.Rate,
                Status = lr2.Status.ToString(),
                DelayInRepayment = lr2.DelayInRepayment,
                ConfirmationDate = lr2.ConfirmationDate
            },
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer2.FirstName,
                LastName = customer2.LastName,
                NationalId = customer2.NationalId,
                PhoneNumber = customer2.PhoneNumber,
                Email = customer2.Email,
                LoanId = lr3.LoanId,
                Rate = lr3.Rate,
                Status = lr3.Status.ToString(),
                DelayInRepayment = lr3.DelayInRepayment,
                ConfirmationDate = lr3.ConfirmationDate
            },
        ]);
    }

    [Fact]
    public async Task GetAllAcceptLoansWithCustomer_return_all_accept_loans_with_customer_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        var customer2 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Accept).WithLoanId(loan.Id).Build();
        var lr2 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Reject).WithLoanId(loan.Id).Build();
        var lr3 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Accept).WithLoanId(loan.Id).Build();
        customer1.LoanRequests.UnionWith([lr1]);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllAcceptLoansWithCustomer();

        // actual.Should().Satisfy(lr => lr.Status == LoanRequestStatus.Active.ToString());
        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer1.FirstName,
                LastName = customer1.LastName,
                NationalId = customer1.NationalId,
                PhoneNumber = customer1.PhoneNumber,
                Email = customer1.Email,
                LoanId = lr1.LoanId,
                Rate = lr1.Rate,
                Status = LoanRequestStatus.Accept.ToString(),
                DelayInRepayment = lr1.DelayInRepayment,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer2.FirstName,
                LastName = customer2.LastName,
                NationalId = customer2.NationalId,
                PhoneNumber = customer2.PhoneNumber,
                Email = customer2.Email,
                LoanId = lr3.LoanId,
                Rate = lr3.Rate,
                Status = LoanRequestStatus.Accept.ToString(),
                DelayInRepayment = lr3.DelayInRepayment,
                ConfirmationDate = lr3.ConfirmationDate
            },
        ]);
    }


    [Fact]
    public async Task GetAllActiveLoansWithCustomer_return_all_active_loans_with_customer_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        var customer2 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        var lr2 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Reject).WithLoanId(loan.Id).Build();
        var lr3 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        customer1.LoanRequests.UnionWith([lr1]);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllActiveLoansWithCustomer();

        // actual.Should().Satisfy(lr => lr.Status == LoanRequestStatus.Active.ToString());
        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer1.FirstName,
                LastName = customer1.LastName,
                NationalId = customer1.NationalId,
                PhoneNumber = customer1.PhoneNumber,
                Email = customer1.Email,
                LoanId = lr1.LoanId,
                Rate = lr1.Rate,
                Status = LoanRequestStatus.Active.ToString(),
                DelayInRepayment = lr1.DelayInRepayment,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer2.FirstName,
                LastName = customer2.LastName,
                NationalId = customer2.NationalId,
                PhoneNumber = customer2.PhoneNumber,
                Email = customer2.Email,
                LoanId = lr3.LoanId,
                Rate = lr3.Rate,
                Status = LoanRequestStatus.Active.ToString(),
                DelayInRepayment = lr3.DelayInRepayment,
                ConfirmationDate = lr3.ConfirmationDate
            },
        ]);
    }

    [Fact]
    public async Task GetAllDelayedLoansWithCustomer_return_all_delayed_loans_with_customer_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        var customer2 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder()
            .WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        var lr2 = new LoanRequestBuilder()
            .WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        var lr3 = new LoanRequestBuilder()
            .WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        customer1.LoanRequests.UnionWith([lr1]);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllDelayedLoansWithCustomer();

        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer1.FirstName,
                LastName = customer1.LastName,
                NationalId = customer1.NationalId,
                PhoneNumber = customer1.PhoneNumber,
                Email = customer1.Email,
                LoanId = lr1.LoanId,
                Rate = lr1.Rate,
                Status = lr1.Status.ToString(),
                DelayInRepayment = true,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer2.FirstName,
                LastName = customer2.LastName,
                NationalId = customer2.NationalId,
                PhoneNumber = customer2.PhoneNumber,
                Email = customer2.Email,
                LoanId = lr3.LoanId,
                Rate = lr3.Rate,
                Status = lr3.Status.ToString(),
                DelayInRepayment = true,
                ConfirmationDate = lr3.ConfirmationDate
            },
        ]);
    }

    [Fact]
    public async Task GetAllCloseLoansWithCustomer_return_all_done_loans_with_customer_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        var customer2 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Close).WithLoanId(loan.Id).Build();
        var lr2 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Close).WithLoanId(loan.Id).Build();
        var lr3 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id).Build();
        customer1.LoanRequests.UnionWith([lr1]);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllClosedLoansWithCustomer();

        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer1.FirstName,
                LastName = customer1.LastName,
                NationalId = customer1.NationalId,
                PhoneNumber = customer1.PhoneNumber,
                Email = customer1.Email,
                LoanId = lr1.LoanId,
                Rate = lr1.Rate,
                Status = LoanRequestStatus.Close.ToString(),
                DelayInRepayment = lr1.DelayInRepayment,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestWithCustomerDto
            {
                FirstName = customer2.FirstName,
                LastName = customer2.LastName,
                NationalId = customer2.NationalId,
                PhoneNumber = customer2.PhoneNumber,
                Email = customer2.Email,
                LoanId = lr2.LoanId,
                Rate = lr2.Rate,
                Status = LoanRequestStatus.Close.ToString(),
                DelayInRepayment = lr2.DelayInRepayment,
                ConfirmationDate = lr2.ConfirmationDate
            },
        ]);
    }

    [Fact]
    public async Task
        GetAllRemainingLoans_return_active_loan_requests_with_their_installments_and_paid_amount_when_invoked()
    {
        var loan = new LoanBuilder().Build();
        Save(loan);
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        var lr1 = new LoanRequestBuilder()
            .WithStatus(LoanRequestStatus.Active).WithLoanId(loan.Id)
            .WithCustomerId(customer1.Id)
            .Build();
        Save(lr1);
        var i1 = new InstallmentBuilder()
            .WithLoanRequest(lr1)
            .WithAmount(100).WithMonthlyInterest(1.5m).WithFine(0)
            .WithPaymentDate(new DateOnly(2024, 1, 5))
            .WithPaymentDeadLine(new DateOnly(2024, 1, 5))
            .Build();
        var i2 = new InstallmentBuilder()
            .WithLoanRequest(lr1)
            .WithAmount(100).WithMonthlyInterest(1.5m).WithFine(2)
            .WithPaymentDate(new DateOnly(2024, 2, 4))
            .WithPaymentDeadLine(new DateOnly(2024, 2, 5))
            .Build();
        var i3 = new InstallmentBuilder()
            .WithLoanRequest(lr1)
            .WithAmount(100).WithMonthlyInterest(1.5m).WithFine(0)
            .WithPaymentDate(new DateOnly(2024, 3, 3))
            .WithPaymentDeadLine(new DateOnly(2024, 3, 5))
            .Build();
        var i4 = new InstallmentBuilder()
            .WithLoanRequest(lr1)
            .WithAmount(100).WithMonthlyInterest(1.5m).WithFine(0)
            .WithPaymentDeadLine(new DateOnly(2024, 3, 5))
            .Build();
        Save(i1, i2, i3, i4);

        var actual = await _sut.GetAllRemainingLoans();

        actual.Should().HaveCount(1);
        actual.Should().BeEquivalentTo([
            new
            {
                Status = LoanRequestStatus.Active.ToString(),
                IsDelayed = lr1.DelayInRepayment,
                TotalPaid = (i1.Fine + i1.Amount + i1.MonthlyInterest) +
                            (i2.Fine + i2.Amount + i3.MonthlyInterest) +
                            (i3.Fine + i2.Amount + i3.MonthlyInterest),
            }
        ]);
        actual[0].Installments.Should().HaveCount(1);
        actual[0].Installments.Should().BeEquivalentTo([
            new GetInstallmentDto
            {
                LoanRequestId = i4.LoanRequestId,
                Amount = i4.Amount,
                MonthlyInterest = i4.MonthlyInterest,
                PaymentDeadLine = i4.PaymentDeadLine,
                PaymentDate = i4.PaymentDate,
                Fine = i4.Fine
            }
        ]);
    }
}
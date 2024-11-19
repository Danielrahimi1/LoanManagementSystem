using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts;
using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts.DTOs;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.UnitOfWorks;
using LoanManagementSystem.TestTools.Application.LoanRequests;
using LoanManagementSystem.TestTools.Services.Customers;
using LoanManagementSystem.TestTools.Services.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Services.LoanRequests;
using LoanManagementSystem.TestTools.Services.Loans;
using Moq;

namespace LoanManagementSystem.Application.Tests.Integration.LoanRequests;

public class PayLoanCommandHandlerTests : BusinessIntegrationTest
{
    private readonly PayLoanHandler _sut;
    private readonly Mock<LoanRequestService> _loanRequestService;
    private readonly Mock<CustomerService> _customerService;

    public PayLoanCommandHandlerTests()
    {
        Mock<UnitOfWork> unitOfWork = new();
        _loanRequestService = new Mock<LoanRequestService>();
        _customerService = new Mock<CustomerService>();
        _sut = PayLoanHandlerFactory.CreateHandler(SetupContext,
            _loanRequestService.Object, _customerService.Object, unitOfWork.Object);
    }

    [Fact]
    public async Task PayLoan_add_loan_amount_to_customer_balance_when_loan_requests_activated()
    {
        var customer = new CustomerBuilder().WithBalance(10).Build();
        Save(customer);
        var loan = new LoanBuilder().WithAmount(100).Build();
        Save(loan);
        var lr = new LoanRequestBuilder()
            .WithLoanId(loan.Id).WithCustomerId(customer.Id)
            .WithStatus(LoanRequestStatus.Accept).WithRate(80)
            .Build();
        Save(lr);
        var cmd = new ActivateLoanRequestCommand
        {
            CustomerId = customer.Id,
            LoanRequestId = lr.Id
        };
        _loanRequestService.Setup(s => s.Activate(cmd.LoanRequestId)).ReturnsAsync(loan.Amount);

        await _sut.Handle(cmd);

        _loanRequestService.Verify(s => s.Activate(cmd.LoanRequestId));
        _customerService.Verify(s => s.Charge(cmd.CustomerId, It.Is<UpdateBalanceDto>(dto =>
            dto.Charge == loan.Amount)));
    }
}
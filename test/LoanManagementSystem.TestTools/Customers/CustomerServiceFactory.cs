using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Persistence.Ef.LoanRequests;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Customers;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;

namespace LoanManagementSystem.TestTools.Customers;

public static class CustomerServiceFactory
{
    public static CustomerService CreateService(EfDataContext context)
    {
        var customerRepository = new EfCustomerRepository(context);
        var loanRequestRepository = new EfLoanRequestRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new CustomerAppService(
            customerRepository,
            loanRequestRepository,
            unitOfWork);
    }
}
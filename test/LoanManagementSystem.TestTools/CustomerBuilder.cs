using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Statements;

namespace LoanManagementSystem.TestTools;

public class CustomerBuilder
{
    private readonly Customer _customer;

    public CustomerBuilder()
    {
        _customer = new Customer
        {
            FirstName = "FName",
            LastName = "LName",
            NationalId = "1239876543",
            PhoneNumber = "9001112233",
            Email = "name@domain.com",
            Balance = 0,
            IsVerified = false,
            CreditScore = 0
        };
    }

    public CustomerBuilder WithFirstName(string value)
    {
        _customer.FirstName = value;
        return this;
    }

    public CustomerBuilder WithLastName(string value)
    {
        _customer.LastName = value;
        return this;
    }

    public CustomerBuilder WithNationalId(string value)
    {
        _customer.NationalId = value;
        return this;
    }

    public CustomerBuilder WithPhoneNumber(string value)
    {
        _customer.PhoneNumber = value;
        return this;
    }

    public CustomerBuilder WithEmail(string value)
    {
        _customer.Email = value;
        return this;
    }

    public CustomerBuilder WithBalance(decimal value)
    {
        _customer.Balance = value;
        return this;
    }

    public CustomerBuilder WithIsVerified(bool value)
    {
        _customer.IsVerified = value;
        return this;
    }

    public CustomerBuilder WithCreditScore(int value)
    {
        _customer.CreditScore = value;
        return this;
    }
    
    public CustomerBuilder WithCreditScore(Statement value)
    {
        _customer.Statement = value;
        return this;
    }

    public Customer Build()
    {
        return _customer;
    }
}
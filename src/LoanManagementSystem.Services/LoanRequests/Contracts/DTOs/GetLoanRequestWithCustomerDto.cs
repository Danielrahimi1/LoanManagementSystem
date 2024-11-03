using System;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;

public class GetLoanRequestWithCustomerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string NationalId { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required int LoanId { get; init; }
    public required int Status { get; set; }
    public bool DelayInRepayment { get; set; }
    public DateOnly? ConfirmationDate { get; set; }
}
using System;
using LoanManagementSystem.Entities.LoanRequests.Enums;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;

public class GetLoanRequestWithCustomerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string NationalId { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required int LoanId { get; init; }
    public required int Rate { get; init; }
    public required string Status { get; set; }
    public required bool DelayInRepayment { get; set; }
    public DateOnly? ConfirmationDate { get; set; }
}
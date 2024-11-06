using System;
using LoanManagementSystem.Entities.LoanRequests.Enums;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;

public class AddLoanRequestDto
{
    public required int LoanId { get; set; }
    public required int CustomerId { get; set; }
}
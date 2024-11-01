namespace LoanManagementSystem.Entities.Loans;

public class Loan
{
    public int Id { get; set; }
    public required decimal Amount { get; set; }
    public required int CountInstallment{ get; set; }
    public required int InterestRateAnnually { get; set; }
}
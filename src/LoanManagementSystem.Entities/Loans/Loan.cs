namespace LoanManagementSystem.Entities.Loans;

public class Loan
{
    public int Id { get; set; }
    public required decimal Amount { get; set; }
    public required int CountInstallment{ get; set; }
    public int InterestRateAnnually { get; set; }
}
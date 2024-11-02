using LoanManagementSystem.Entities.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagementSystem.Persistence.Ef.Loans;

public class LoanEntityMap : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");
        builder.HasKey(loan => loan.Id);
        builder.Property(loan => loan.Id).UseIdentityColumn();
        builder.Property(loan => loan.Amount).HasColumnType("money");
    }
}
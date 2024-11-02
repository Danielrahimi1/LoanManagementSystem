using LoanManagementSystem.Entities.Installments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagementSystem.Persistence.Ef.Installments;

public class InstallmentEntityMap : IEntityTypeConfiguration<Installment>
{
    public void Configure(EntityTypeBuilder<Installment> builder)
    {
        builder.ToTable("Installments");
        builder.HasKey(installment => installment.Id);
        builder.Property(installment => installment.Id).UseIdentityColumn();
        builder.Property(installment => installment.Amount).HasColumnType("money");
        builder.Property(installment => installment.Fine).HasColumnType("money");
        builder.Property(installment => installment.PaymentDate).IsRequired(false);
        builder.Property(installment => installment.MonthlyInterestRate).HasColumnType("money");
    }
}
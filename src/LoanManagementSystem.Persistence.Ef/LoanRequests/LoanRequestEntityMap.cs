using LoanManagementSystem.Entities.LoanRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagementSystem.Persistence.Ef.LoanRequests;

public class LoanRequestEntityMap : IEntityTypeConfiguration<LoanRequest>
{
    public void Configure(EntityTypeBuilder<LoanRequest> builder)
    {
        builder.ToTable("LoanRequests");
        builder.HasKey(lr => lr.Id);
        builder.Property(lr => lr.Id).UseIdentityColumn();
        builder.Property(lr => lr.ConfirmationDate).IsRequired(false);
        builder.HasMany(lr => lr.Installments).WithOne(installment => installment.LoanRequest)
            .HasForeignKey(installment => installment.LoanRequestId);
    }
}
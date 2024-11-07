using System.Data;
using FluentMigrator;

namespace Migration.Migrations;

[Migration(5)]
public class AddInstallmentsTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Installments")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Amount").AsDecimal(19, 4).NotNullable()
            .WithColumn("MonthlyInterest").AsDecimal(19,4).NotNullable()
            .WithColumn("PaymentDeadLine").AsDate().NotNullable()
            .WithColumn("PaymentDate").AsDate().Nullable()
            .WithColumn("Fine").AsDecimal(19,4).NotNullable()
            .WithColumn("LoanRequestId").AsInt32().NotNullable()
                .ForeignKey("FK_Installments_LoanRequests","LoanRequests","Id")
                .OnDelete(Rule.None);
    }

    public override void Down()
    {
        Delete.Table("Installments");
    }
}
using System.Data;
using FluentMigrator;

namespace Migration.Migrations;

[Migration(3)]
public class AddLoanRequestsTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("LoanRequests")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Status").AsInt32().NotNullable()
            .WithColumn("DelayInRepayment").AsBoolean().NotNullable()
            .WithColumn("ConfirmationDate").AsDate().Nullable()
            .WithColumn("Rate").AsInt32().NotNullable()
            .WithColumn("LoanId").AsInt32().NotNullable()
            .WithColumn("CustomerId").AsInt32().NotNullable()
                .ForeignKey("FK_LoanRequests_Customers", "Customers", "Id")
                .OnDelete(Rule.None);
    }

    public override void Down()
    {
        Delete.Table("LoanRequests");
    }
}
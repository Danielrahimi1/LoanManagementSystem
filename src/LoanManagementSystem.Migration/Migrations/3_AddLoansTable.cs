using System.Data;
using FluentMigrator;

namespace Migration.Migrations;

[Migration(3)]
public class AddLoansTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Loans")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Amount").AsDecimal(19, 6).NotNullable()
            .WithColumn("InstallmentCount").AsInt32().NotNullable()
            .WithColumn("AnnualInterestRate").AsInt32().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Loans");
    }
}
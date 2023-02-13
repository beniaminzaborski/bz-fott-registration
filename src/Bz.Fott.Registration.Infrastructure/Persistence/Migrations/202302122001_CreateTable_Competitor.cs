using FluentMigrator;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Migrations;

[Migration(202302122001)]
public class _202302122001_CreateTable_Competitor : Migration
{
    public override void Down()
    {
        Delete.Table("competitors");
    }

    public override void Up()
    {
        Create.Table("competitors")
           .WithColumn("id").AsGuid().NotNullable()
           .WithColumn("requestId").AsGuid().NotNullable()
           .WithColumn("competitionId").AsGuid().NotNullable()
           .WithColumn("number").AsString(6).NotNullable()
           .WithColumn("firstName").AsString(100).NotNullable()
           .WithColumn("lastName").AsString(150).NotNullable()
           .WithColumn("birthDate").AsDate().NotNullable()
           .WithColumn("city").AsString(100).NotNullable()
           .WithColumn("phoneNumber").AsString(17).NotNullable()
           .WithColumn("contactPersonNumber").AsString(17).NotNullable();

        Create.PrimaryKey($"PK__competitors__id")
            .OnTable("competitors").Column("id");

        Create.UniqueConstraint("unq__competitionId__number")
            .OnTable("competitors")
            .Columns("competitionId", "number");
    }
}

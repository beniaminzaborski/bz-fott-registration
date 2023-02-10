using FluentMigrator;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Migrations;

[Migration(202302101657)]
public class _202302101657_CreateTable_Competition : Migration
{
    public override void Down()
    {
        Delete.Table("competitions");
    }

    public override void Up()
    {
        Create.Table("competitions")
            .WithColumn("id").AsGuid().NotNullable()
            .WithColumn("maxCompetitors").AsInt32().NotNullable()
            .WithColumn("isRegistrationOpen").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.PrimaryKey($"PK__competitions__id")
            .OnTable("competitions").Column("id");
    }
}

using FluentMigrator;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Migrations;

[Migration(202304241831)]
public class _202304241831_AlterTable_Competitor_Add_NetTime : Migration
{
    public override void Down()
    {
        Delete.Column("netTime").FromTable("competitors");
    }

    public override void Up()
    {
        Alter.Table("competitors").AddColumn("netTime").AsDateTimeOffset().Nullable();
    }
}

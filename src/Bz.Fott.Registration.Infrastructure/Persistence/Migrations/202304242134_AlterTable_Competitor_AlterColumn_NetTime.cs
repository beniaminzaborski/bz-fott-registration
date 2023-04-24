using FluentMigrator;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Migrations;

[Migration(202304242134)]
public class _202304242134_AlterTable_Competitor_AlterColumn_NetTime : Migration
{
    public override void Down()
    {
        Delete.Column("netTime").FromTable("competitors");
        Alter.Table("competitors").AddColumn("netTime").AsDateTimeOffset().Nullable();
    }

    public override void Up()
    {
        Delete.Column("netTime").FromTable("competitors");
        Alter.Table("competitors").AddColumn("netTime").AsInt64().Nullable();
    }
}

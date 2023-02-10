using FluentMigrator;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Migrations;

[Maintenance(MigrationStage.BeforeAll, TransactionBehavior.Default)]
public class DbMigrationLockBefore : Migration
{
    public override void Up()
    {
        Execute.Sql("select pg_advisory_lock(666);");
    }

    public override void Down()
    {
        throw new NotImplementedException("Down migrations are not supported for sp_getapplock");
    }
}

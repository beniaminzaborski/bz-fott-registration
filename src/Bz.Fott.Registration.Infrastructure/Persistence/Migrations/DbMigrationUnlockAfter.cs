using FluentMigrator;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Migrations;

[Maintenance(MigrationStage.AfterAll, TransactionBehavior.Default)]
public class DbMigrationUnlockAfter : Migration
{
    public override void Up()
    {
        Execute.Sql("select pg_advisory_unlock(666);");
    }

    public override void Down()
    {
        throw new NotImplementedException("Down migrations are not supported for sp_releaseapplock");
    }
}

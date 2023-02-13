using FluentMigrator;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Migrations;

[Migration(202302121802)]
public class _202302121802_CreateTable_Numerator : Migration
{
    public override void Down()
    {
        Delete.Table("numerators");
    }

    public override void Up()
    {
        Create.Table("numerators")
           .WithColumn("id").AsGuid().NotNullable()
           .WithColumn("competitionId").AsGuid().NotNullable().Unique()
           .WithColumn("currentNumber").AsInt64().NotNullable();

        Create.PrimaryKey($"PK__numerators__id")
            .OnTable("numerators").Column("id");

        Execute.Sql(@"
            CREATE OR REPLACE PROCEDURE generate_next_number(competitionIdParam uuid)
            LANGUAGE SQL
            AS $$

            INSERT INTO numerators
                (id, competitionId, lastNumber)
            SELECT gen_random_uuid(), competitionIdParam, 0
            WHERE
                NOT EXISTS (
                    SELECT id FROM numerators WHERE competitionId = competitionIdParam
                );

            UPDATE numerators 
	            SET lastNumber = lastNumber + 1 
            WHERE competitionId = competitionIdParam;

            $$;");
    }
}

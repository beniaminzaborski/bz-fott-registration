﻿using FluentMigrator;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Migrations;

[Migration(202302121803)]
public class _202302121803_CreateProcedure_GenerateNextNumber : Migration
{
    public override void Down()
    {
        Execute.Sql(@"DROP PROCEDURE generate_next_number;");
    }

    public override void Up()
    {
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

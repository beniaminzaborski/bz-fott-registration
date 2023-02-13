using Azure.Core;
using Bz.Fott.Registration.Application.CompetitorRegistration;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using System;

namespace Bz.Fott.Registration.NumberAssignatorAzFunction;

public class NumberAssignatorFunction
{
    [FunctionName("NumberAssignator")]
    public void Run([ServiceBusTrigger("registrations", Connection = "ServiceBusConnectionString")]RegisterCompetitor registerCompetitor, ILogger log)
    {
        log.LogInformation($"C# ServiceBus queue trigger function processed message: {registerCompetitor.RequestId}, {registerCompetitor.CompetitionId}, {registerCompetitor.FirstName}");

        var connectionString = Environment.GetEnvironmentVariable("PostgresConnectionString", EnvironmentVariableTarget.Process);
        log.LogInformation($"connectionString is {connectionString}");

        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();
        var tran = conn.BeginTransaction();
        try
        {
            // Generate next Number
            using var numberGeneratorCmd = new NpgsqlCommand(@"
            call generate_next_number(@competitionId);
            select lastNumber from numerators where competitionId = @competitionId;
            ", conn, tran);
            numberGeneratorCmd.Parameters.AddWithValue("competitionId", registerCompetitor.CompetitionId);
            var number = (long)numberGeneratorCmd.ExecuteScalar();
            log.LogInformation($"Get the Number: {number}");

            // Insert into Competitors table
            using var insertCompetitorCmd = new NpgsqlCommand(@"
            INSERT INTO competitors (id, competitionId, number, firstName, lastName, birthDate, city, phoneNumber, contactPersonNumber) 
            VALUES (@id, @competitionId, @number, @firstName, @lastName, @birthDate, @city, @phoneNumber, @contactPersonNumber)", conn, tran);
            insertCompetitorCmd.Parameters.AddWithValue("id", Guid.NewGuid());
            insertCompetitorCmd.Parameters.AddWithValue("requestId", registerCompetitor.RequestId);
            insertCompetitorCmd.Parameters.AddWithValue("competitionId", registerCompetitor.CompetitionId);
            insertCompetitorCmd.Parameters.AddWithValue("firstName", registerCompetitor.FirstName);
            insertCompetitorCmd.Parameters.AddWithValue("lastName", registerCompetitor.LastName);
            insertCompetitorCmd.Parameters.AddWithValue("birthDate", registerCompetitor.BirthDate);
            insertCompetitorCmd.Parameters.AddWithValue("city", registerCompetitor.City);
            insertCompetitorCmd.Parameters.AddWithValue("phoneNumber", registerCompetitor.PhoneNumber);
            insertCompetitorCmd.Parameters.AddWithValue("contactPersonNumber", registerCompetitor.ContactPersonNumber);
            insertCompetitorCmd.ExecuteNonQuery();

            tran.Commit();
        }
        catch (Exception ex)
        {
            log.LogError($"Something went wrong for request id: {registerCompetitor.RequestId}. Details: {ex.Message}");
            tran.Rollback();
        }
    }
}

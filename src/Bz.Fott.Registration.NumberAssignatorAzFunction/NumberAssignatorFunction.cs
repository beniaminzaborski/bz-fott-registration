using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using Azure.Messaging.ServiceBus;
using Bz.Fott.Registration.Application.CompetitorRegistration;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Text;
using Dapper;

namespace Bz.Fott.Registration.NumberAssignatorAzFunction;

public class NumberAssignatorFunction
{
    [FunctionName("NumberAssignator")]
    public void Run([ServiceBusTrigger("registrations", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage receivedMessage, ILogger log)
    {
        var registerCompetitor = GetMessageContent<RegisterCompetitor>(receivedMessage);

        using var connection = CreateConnection();
        var transaction = connection.BeginTransaction();
        try
        {
            var number = GetNextNumber(registerCompetitor.CompetitionId);
            InsertCompetitor(registerCompetitor, number);
            transaction.Commit();
        }
        catch (Exception ex)
        {
            log.LogError($"Something went wrong for request id: {registerCompetitor.RequestId}. Details: {ex.Message}");
            transaction.Rollback();
        }

        static T GetMessageContent<T>(ServiceBusReceivedMessage receivedMessage)
        {
            string payload = Encoding.UTF8.GetString(receivedMessage.Body);
            var json = JsonNode.Parse(payload)["message"].ToString();
            return JsonConvert.DeserializeObject<T>(json);
        }

        NpgsqlConnection CreateConnection()
        {
            var connectionString = Environment.GetEnvironmentVariable("PostgresConnectionString", EnvironmentVariableTarget.Process);
            return new NpgsqlConnection(connectionString);
        }

        long GetNextNumber(Guid competitionId)
        {
            var sql = @"
                call generate_next_number(@competitionId);
                select lastNumber from numerators where competitionId = @competitionId;";
            return connection.ExecuteScalar<long>(sql, new { competitionId }, transaction);
        }

        void InsertCompetitor(RegisterCompetitor registerCompetitor, long number)
        {
            var sql = @"
                INSERT INTO competitors (""id"", ""requestId"", ""competitionId"", ""number"", ""firstName"", ""lastName"", ""birthDate"", ""city"", ""phoneNumber"", ""contactPersonNumber"") 
                VALUES (@id, @requestId, @competitionId, @number, @firstName, @lastName, @birthDate, @city, @phoneNumber, @contactPersonNumber)";
            connection.Execute(sql, new 
            { 
                id = Guid.NewGuid(),
                requestId = registerCompetitor.RequestId,
                competitionId = registerCompetitor.CompetitionId,
                number,
                firstName = registerCompetitor.FirstName,
                lastName = registerCompetitor.LastName,
                birthDate = registerCompetitor.BirthDate,
                city = registerCompetitor.City,
                phoneNumber = registerCompetitor.PhoneNumber,
                contactPersonNumber = registerCompetitor.ContactPersonNumber
            }, transaction);
        }
    }
}

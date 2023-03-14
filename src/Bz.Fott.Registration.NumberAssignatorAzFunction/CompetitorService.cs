using Bz.Fott.Registration.Application.CompetitorRegistration;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Bz.Fott.Registration.NumberAssignatorAzFunction;

internal class CompetitorService : ICompetitorService
{
    readonly ILogger<CompetitorService> _logger;

    public CompetitorService(ILogger<CompetitorService> logger)
    {
        _logger = logger;
    }

    public async Task<string> RegisterCompetitorAndReturnNumber(RegisterCompetitor competitor)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();
        var transaction = await connection.BeginTransactionAsync();
        try
        {
            var number = await GetNextNumberAsync(competitor.CompetitionId);
            await InsertCompetitorAsync(competitor, number);
            await transaction.CommitAsync();
            return number.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong for request id: {competitor.RequestId}. Details: {ex.Message}");
            await transaction.RollbackAsync();
        }

        return null;

        async Task<long> GetNextNumberAsync(Guid competitionId)
        {
            var sql = @"
                call generate_next_number(@competitionId);
                select ""currentNumber"" from numerators where ""competitionId"" = @competitionId;";
            return await connection.ExecuteScalarAsync<long>(sql, new { competitionId }, transaction);
        }

        async Task InsertCompetitorAsync(RegisterCompetitor registerCompetitor, long number)
        {
            var sql = @"
                INSERT INTO competitors (""id"", ""requestId"", ""competitionId"", ""number"", ""firstName"", ""lastName"", ""birthDate"", ""city"", ""phoneNumber"", ""contactPersonNumber"") 
                VALUES (@id, @requestId, @competitionId, @number, @firstName, @lastName, @birthDate, @city, @phoneNumber, @contactPersonNumber)";
            await connection.ExecuteAsync(sql, new
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

        NpgsqlConnection CreateConnection()
        {
            var connectionString = Environment.GetEnvironmentVariable("PostgresConnectionString", EnvironmentVariableTarget.Process);
            return new NpgsqlConnection(connectionString);
        }
    }
}

namespace Bz.Fott.Registration.Application.Competitors;

public record CompetitorDto
{
    public Guid Id { get; init; }
    public Guid CompetitionId { get; init; }
    public string Number { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime BirthDate { get; init; }
    public string City { get; init; }
    public TimeSpan? NetTime { get; init; }
}

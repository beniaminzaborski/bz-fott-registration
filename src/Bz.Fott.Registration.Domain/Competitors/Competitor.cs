using Bz.Fott.Registration.Domain.Common;
using Bz.Fott.Registration.Domain.CompetitionIntegration;

namespace Bz.Fott.Registration.Domain.Competitors;

public class Competitor : Entity<CompetitorId>, IAggregateRoot
{
    private Competitor() { }

    public Competitor(
       CompetitorId id,
       CompetitionId competitionId,
       string number,
       string firstName,
       string lastName,
       DateTime birthDate,
       string city,
       string phoneNumber,
       string contactPersonNumber)
    {
        Id= id;
        CompetitionId = competitionId;
        Number = number;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        City = city;
        PhoneNumber = phoneNumber;
        ContactPersonNumber = contactPersonNumber;
    }

    public CompetitionId CompetitionId { get; private set; }

    public string Number { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public DateTime BirthDate { get; private set; }

    public string City { get; private set; }

    public string PhoneNumber { get; private set; }

    public string ContactPersonNumber { get; private set; }
}

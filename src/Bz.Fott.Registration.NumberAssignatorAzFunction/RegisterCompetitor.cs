using System;

namespace Bz.Fott.Registration.Application.CompetitorRegistration;

public class RegisterCompetitor
{
    public Guid RequestId { get; set; }
    public Guid CompetitionId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string City { get; set; }
    public string PhoneNumber { get; set; }
    public string ContactPersonNumber { get; set; }
}

using Bz.Fott.Registration.Domain.Common;

namespace Bz.Fott.Registration.Domain.CompetitionIntegration;

public record CompetitionId : EntityId<Guid>
{
    public CompetitionId(Guid value) : base(value) { }

    public static CompetitionId From(Guid value)
    { 
        return new CompetitionId(value);
    }
}

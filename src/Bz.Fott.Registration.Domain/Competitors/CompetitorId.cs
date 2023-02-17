using Bz.Fott.Registration.Domain.Common;

namespace Bz.Fott.Registration.Domain.Competitors;

public record CompetitorId : EntityId<Guid>
{
    public CompetitorId(Guid value) : base(value) { }

    public static CompetitorId From(Guid value)
    {
        return new CompetitorId(value);
    }
}

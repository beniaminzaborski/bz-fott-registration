using Bz.Fott.Registration.Domain.CompetitionIntegration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Bz.Fott.Registration.Domain.Competitors;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Mappings;

internal class CompetitorMapping : IEntityTypeConfiguration<Competitor>
{
    public void Configure(EntityTypeBuilder<Competitor> builder)
    {
        builder.ToTable(name: "competitors");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(entityId => entityId.Value, dbId => new CompetitorId(dbId));

        builder.Property(e => e.CompetitionId)
            .HasColumnName("competitionId")
            .HasConversion(entityId => entityId.Value, dbId => new CompetitionId(dbId));

        builder.Property(e => e.Number).HasColumnName("number");
        builder.Property(e => e.FirstName).HasColumnName("firstName");
        builder.Property(e => e.LastName).HasColumnName("lastName");
        builder.Property(e => e.BirthDate).HasColumnName("birthDate");
        builder.Property(e => e.City).HasColumnName("city");
        builder.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");
        builder.Property(e => e.ContactPersonNumber).HasColumnName("contactPersonNumber");
        
        builder.Property(e => e.NetTime)
            .HasColumnName("netTime")
            .HasConversion<long?>(entityValue => entityValue.HasValue ? entityValue.Value.Ticks : null, dbValue => dbValue.HasValue ? TimeSpan.FromTicks(dbValue.Value) : null);
    }
}

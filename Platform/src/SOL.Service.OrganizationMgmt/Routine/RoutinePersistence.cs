using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Service.OrganizationMgmt.Routine.Domain;

namespace SOL.Service.OrganizationMgmt.Routine;

class RoutinePersistence : IEntityTypeConfiguration<Domain.Routine>
{
    public void Configure(EntityTypeBuilder<Domain.Routine> builder)
    {
        builder.ToTable("Routine");
        builder.Ignore(x => x.Changes);

        builder.Property(x => x.Name);
        builder.Property(x => x.Active);
        builder.Property(x => x.PurposeId);
        builder.Property(x => x.Settings);
        builder.Property(x => x.Description);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ModifiedAt);

        builder.OwnsOne(x => x.Rolling, oeb => {
            oeb.WithOwner()
                .HasForeignKey("RoutineId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("RoutineRolling");

            oeb.Property<Guid>("RoutineId");
            oeb.OwnsOne(x => x.Interval, ieb => {
                ieb.Property(x => x.Value).HasColumnName("IntervalValue");
                ieb.Property(x => x.Unit).HasColumnName("IntervalUnit");
                ieb.UsePropertyAccessMode(PropertyAccessMode.Property);
            });
            oeb.OwnsOne(x => x.Delay, ieb => {
                ieb.Property(x => x.Value).HasColumnName("DelayValue");
                ieb.Property(x => x.Unit).HasColumnName("DelayUnit");
                ieb.UsePropertyAccessMode(PropertyAccessMode.Property);
            });

            oeb.HasKey("RoutineId");
            oeb.UsePropertyAccessMode(PropertyAccessMode.Property);
        });

        builder.OwnsMany(x => x.Recurring, oeb => {
            oeb.WithOwner()
                .HasForeignKey("RoutineId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("RoutineRecurrence");

            oeb.Property<Guid>("RoutineId");
            oeb.Property(x => x.Time);
            oeb.Property(x => x.Days);

            oeb.HasKey("RoutineId", nameof(Recurrence.Time), nameof(Recurrence.Days));
        });

        builder.Navigation(x => x.Recurring)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_recurring");

        builder.OwnsMany(x => x.Origins, oeb => {
            oeb.WithOwner()
                .HasForeignKey("RoutineId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("RoutineOrigin");

            oeb.Property<Guid>("RoutineId");
            oeb.Property(x => x.ProcedureId);
            oeb.Property(x => x.PropertyId);
            oeb.Property(x => x.PropertyValue);

            oeb.HasKey("RoutineId", nameof(Trigger.ProcedureId));
        });

        builder.Navigation(x => x.Origins)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_origins");

        builder.OwnsMany(x => x.Termini, oeb => {
            oeb.WithOwner()
                .HasForeignKey("RoutineId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("RoutineTerminus");

            oeb.Property<Guid>("RoutineId");
            oeb.Property(x => x.ProcedureId);
            oeb.Property(x => x.PropertyId);
            oeb.Property(x => x.PropertyValue);

            oeb.HasKey("RoutineId", nameof(Trigger.ProcedureId));
        });

        builder.Navigation(x => x.Termini)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_termini");
    }
}

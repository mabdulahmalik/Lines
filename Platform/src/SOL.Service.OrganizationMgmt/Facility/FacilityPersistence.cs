using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;
using SOL.Service.OrganizationMgmt.Facility.Domain;

namespace SOL.Service.OrganizationMgmt.Facility;

public class FacilityPersistence : IEntityTypeConfiguration<Domain.Facility>
{
    public void Configure(EntityTypeBuilder<Domain.Facility> builder)
    {
        builder.ToTable("Facility");
        builder.Ignore(x => x.Changes);

        builder.Property(x => x.Name).HasMaxLength(80);
        builder.Property(x => x.TimeZone).HasMaxLength(50);
        builder.Property(x => x.Order);
        builder.Property(x => x.TypeId);
        builder.Property(x => x.Archived);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ModifiedAt);
        builder.Property(x => x.RequiredData);
        builder.ComplexProperty(x => x.Address, cpb => {
            cpb.Property(x => x.Street).HasColumnName(nameof(Address.Street)).HasMaxLength(80);
            cpb.Property(x => x.City).HasColumnName(nameof(Address.City)).HasMaxLength(30);
            cpb.Property(x => x.State).HasColumnName(nameof(Address.State)).HasMaxLength(2);
            cpb.Property(x => x.ZipCode).HasColumnName(nameof(Address.ZipCode)).HasMaxLength(5);
        });

        builder.HasKey(x => x.Id);

        builder.OwnsMany<TimeStamped<Guid>>("_purposeIds", oeb => {
            oeb.WithOwner()
                .HasForeignKey("FacilityId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("FacilityPurpose");

            oeb.Property<Guid>("FacilityId");
            oeb.Property(x => x.TimeStamp);
            oeb.Property(x => x.Value).HasColumnName("PurposeId");

            oeb.HasKey("FacilityId", nameof(TimeStamped<Guid>.TimeStamp), nameof(TimeStamped<Guid>.Value));
            oeb.UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.OwnsMany<TimeStamped<Guid>>("_procedureIds", oeb => {
            oeb.WithOwner()
                .HasForeignKey("FacilityId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("FacilityProcedure");

            oeb.Property<Guid>("FacilityId");
            oeb.Property(x => x.TimeStamp);
            oeb.Property(x => x.Value).HasColumnName("ProcedureId");

            oeb.HasKey("FacilityId", nameof(TimeStamped<Guid>.TimeStamp), nameof(TimeStamped<Guid>.Value));
            oeb.UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.OwnsMany(x => x.Routines, oeb => {
            oeb.WithOwner()
                .HasForeignKey("FacilityId")
                .HasPrincipalKey(x => x.Id);

            oeb.ToTable("FacilityRoutine");

            oeb.Property<Guid>("FacilityId");
            oeb.Property(x => x.RoutineId);
            oeb.Property(x => x.Name);
            
            oeb.HasKey(x => x.Id);

            oeb.OwnsMany<AssignmentSpec>("_specs", sp => {
                sp.WithOwner()
                    .HasForeignKey("FacilityRoutineId")
                    .HasPrincipalKey(x => x.Id);

                sp.ToTable("FacilityRoutineSpec");

                sp.Property<Guid>("FacilityRoutineId");
                sp.Property(x => x.PropertyId);
                sp.Property(x => x.PropertyValue);

                sp.HasKey("FacilityRoutineId", nameof(AssignmentSpec.PropertyId));
                sp.UsePropertyAccessMode(PropertyAccessMode.Field);
            });
        });

        builder.Navigation(x => x.Routines)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_assignments");
    }
}

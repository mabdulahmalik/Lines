using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.PatientEncounter.Procedure;

class ProcedurePersistence : IEntityTypeConfiguration<Domain.Procedure>
{
    public void Configure(EntityTypeBuilder<Domain.Procedure> builder)
    {
        builder.ToTable("Procedure");
        builder.Ignore(x => x.Changes);

        builder.Property(x => x.Name).HasMaxLength(255);
        builder.Property(x => x.Order);
        builder.Property(x => x.Settings);
        builder.Property(x => x.Archived);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ModifiedAt);
        builder.Property(x => x.RequiredData);

        builder.OwnsMany(x => x.Fields, oeb => {
            oeb.WithOwner()
                .HasForeignKey("ProcedureId")
                .HasPrincipalKey(x => x.Id);

           oeb.ToTable("ProcedureField");

           oeb.Property<Guid>("ProcedureId");
           oeb.Property(x => x.Name);
           oeb.Property(x => x.Type);
           oeb.Property(x => x.Instruction);
           oeb.Property(x => x.Settings);
           oeb.Property(x => x.Archived);

           oeb.HasKey(x => x.Id);
           oeb.UsePropertyAccessMode(PropertyAccessMode.Field);

           oeb.OwnsMany(x => x.Options, opts => {
                opts.WithOwner()
                    .HasForeignKey("FieldId")
                    .HasPrincipalKey(x => x.Id);

                opts.ToTable("ProcedureFieldOption");

                opts.Property<Guid>("FieldId");
                opts.Property(x => x.Text).HasMaxLength(255);
                opts.Property(x => x.Archived);

                opts.HasKey(x => x.Id);
                opts.UsePropertyAccessMode(PropertyAccessMode.Field);
           });

           oeb.Navigation(x => x.Options)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_options");
        });

        builder.Navigation(x => x.Fields)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_fields");
    }
}
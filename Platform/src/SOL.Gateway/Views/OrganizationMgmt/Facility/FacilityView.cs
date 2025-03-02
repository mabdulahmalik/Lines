using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.OrganizationMgmt.Facility;

public class FacilityView : IEntityTypeConfiguration<FacilityView>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? TimeZone { get; set; }
    public Guid? TypeId { get; set; }
    public int Order { get; set; }
    public int? ReferenceCount { get; set; }
    public bool? Archived { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Address? Address { get; set; }
    public RequiredPatientData RequiredData { get; set; }
    
    public void Configure(EntityTypeBuilder<FacilityView> builder)
    {
        builder.ToView("vw_Facility", LinesDataStore.DbSchema.OrganizationMgmt);
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Address, cpb =>
        {
            cpb.Property(x => x.Street).HasColumnName(nameof(Address.Street)).HasMaxLength(80);
            cpb.Property(x => x.City).HasColumnName(nameof(Address.City)).HasMaxLength(30);
            cpb.Property(x => x.State).HasColumnName(nameof(Address.State)).HasMaxLength(2);
            cpb.Property(x => x.ZipCode).HasColumnName(nameof(Address.ZipCode)).HasMaxLength(5);
        });
    }
}
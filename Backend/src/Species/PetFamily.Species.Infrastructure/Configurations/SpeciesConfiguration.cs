using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Species.Infrastructure.Configurations;

public class SpeciesConfiguration: IEntityTypeConfiguration<Domain.Models.Species>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Species> builder)
    {
        builder.ToTable("species");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion( id => id.Value,
                value => SpeciesId.Create(value));
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.Title)
            .IsRequired()
            .HasColumnName("title")
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

        builder.HasMany(p => p.Breeds)
            .WithOne()
            .HasForeignKey("species_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
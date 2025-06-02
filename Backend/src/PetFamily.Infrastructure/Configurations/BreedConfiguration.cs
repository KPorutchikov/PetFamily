using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Species;
using PetFamily.Domain.Species.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Specieses;

namespace PetFamily.Infrastructure.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breed");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
               .HasConversion(id => id.Value,
                value => BreedId.Create(value));

        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
    }
}
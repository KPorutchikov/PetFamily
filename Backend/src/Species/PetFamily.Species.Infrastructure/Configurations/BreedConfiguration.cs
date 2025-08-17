using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Domain;
using PetFamily.Species.Domain.Models;

namespace PetFamily.Species.Infrastructure.Configurations;

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
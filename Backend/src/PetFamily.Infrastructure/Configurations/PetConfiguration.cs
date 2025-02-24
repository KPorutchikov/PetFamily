using PetFamily.Domain.Volunteers;
using PetFamily.Domain.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration: IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion( id => id.Value,
                value => PetId.Create(value));

        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.ComplexProperty(p => p.Breed, r =>
        {
            r.Property(s => s.SpeciesId)
                .IsRequired()
                .HasColumnName("species_id");

            r.Property(b => b.BreedId)
                .IsRequired()
                .HasColumnName("breed_id");
        });
        
        builder.Property(p => p.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

        builder.Property(p => p.Color)
            .IsRequired()
            .HasColumnName("color")
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(p => p.Weight)
            .IsRequired()
            .HasColumnName("weight");
        
        builder.Property(p => p.Height)
            .IsRequired()
            .HasColumnName("height");

        builder.Property(p => p.HealthInformation)
            .IsRequired()
            .HasColumnName("health_information")
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

        builder.ComplexProperty(p => p.Address, a =>
        {
            a.Property(x => x.City)
                .IsRequired()
                .HasColumnName("address_city")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            a.Property(x => x.Street)
                .IsRequired()
                .HasColumnName("address_street")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            a.Property(x => x.HouseNumber)
                .IsRequired(false)
                .HasColumnName("address_house_number");
            
            a.Property(x => x.ApartmentNumber)
                .IsRequired(false)
                .HasColumnName("address_apartment_number");
        });
        
        builder.Property(p => p.Phone)
            .IsRequired()
            .HasColumnName("phone")
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.IsCastrated)
            .IsRequired()
            .HasColumnName("is_castrated");

        builder.Property(p => p.BirthDate)
            .IsRequired()
            .HasColumnName("birthdate");

        builder.Property(p => p.IsVaccinated)
            .IsRequired()
            .HasColumnName("is_vaccinated");

        builder.ComplexProperty(p => p.Status, s =>
        {
            s.Property(x => x.Value)
                .IsRequired()
                .HasColumnName("status");
        });

        builder.ComplexProperty(p => p.Requisites, r =>
        {
            r.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("requisites_name")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            
            r.Property(x => x.Description)
                .IsRequired()
                .HasColumnName("requisites_description")
                .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);
        });
        
        builder.Property(p => p.CreatedDate)
            .IsRequired()
            .HasColumnName("created_date");
    }
}
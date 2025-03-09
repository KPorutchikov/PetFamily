using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Volunteers;
using PetFamily.Domain.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;


namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => VolunteerId.Create(value));

        builder.Property(p => p.FullName)
            .IsRequired()
            .HasColumnName("full_name")
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.ComplexProperty(p => p.Email, e =>
        {
            e.Property(p => p.Value)
                .IsRequired(true)
                .HasColumnName("email")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.Property(p => p.Phone)
            .IsRequired()
            .HasColumnName("phone")
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasColumnName("description")
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

        builder.Property(p => p.ExperienceInYears)
            .IsRequired(false)
            .HasColumnName("experience_in_years");

        builder.HasMany(p => p.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(p => p.RequisitesDetails, r =>
        {
            r.ToJson("requisites_details");
            r.OwnsMany(x => x.RequisitesList, n =>
            {
                n.Property(v => v.Name)
                    .IsRequired()
                    .HasColumnName("requisites_name")
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
                n.Property(v => v.Description)
                    .IsRequired()
                    .HasColumnName("requisites_description")
                    .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);
            });
        });

        builder.OwnsOne(p => p.SocialNetworkDetails, s =>
        {
            s.ToJson("social_networks");
            s.OwnsMany(x => x.SocialNetworks, n =>
            {
                n.Property(l => l.Link)
                    .IsRequired()
                    .HasColumnName("social_networks_link")
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
                n.Property(l => l.Title)
                    .IsRequired(false)
                    .HasColumnName("social_networks_title")
                    .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);
            });
        });
    }
}
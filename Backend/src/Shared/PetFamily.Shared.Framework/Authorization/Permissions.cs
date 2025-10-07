namespace PetFamily.Shared.Framework.Authorization;

public static class Permissions
{
    public static class Volunteer
    {
        public const string VolunteerRead   = "volunteer.read";
        public const string VolunteerCreate = "volunteer.create";
        public const string VolunteerUpdate = "volunteer.update";
        public const string VolunteerDelete = "volunteer.delete";
        public const string VolunteerMove   = "volunteer.move";
    }
    
    public static class Species
    {
        public const string SpeciesRead   = "species.read";
        public const string SpeciesCreate = "species.create";
        public const string SpeciesUpdate = "species.update";
        public const string SpeciesDelete = "species.delete";
        public const string SpeciesMove   = "species.move";
    }
}
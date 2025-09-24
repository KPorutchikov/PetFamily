namespace PetFamily.Shared.SharedKernel;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var val = name ?? "Value";
            return Error.Validation("value.is.invalid", $"{val} is invalid");
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for id '{id}'";
            return Error.NotFound("record.not.found", $"record not found{forId}");
        }
        
        public static Error ValueIsRequired(string? name = null)
        {
            var val = name == null ? " " : " " + name + " ";
            return Error.Validation("length.is.invalid", $"invalid{val}length");
        }
        
        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "record already exist");
        }
    }

    public static class Volunteer
    {
        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "volunteer already exist");
        }
    }
    
    public static class Species
    {
        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "species already exist");
        }
    }
    
    public static class Breed
    {
        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "breed already exist");
        }
    }
}
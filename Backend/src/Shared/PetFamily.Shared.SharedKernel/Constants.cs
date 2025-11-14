namespace PetFamily.Shared.SharedKernel;

public class Constants
{
    public const string BUCKET_NAME = "files";
    public const int MAX_LOW_TEXT_LENGTH = 100;
    public const int MAX_MEDIUM_TEXT_LENGTH = 1000;
}

public enum Modules
{
    Volunteers,
    Species,
    Accounts
}

public enum UserAccountType
{
    Admin,
    Volunteers,
    Participant
}
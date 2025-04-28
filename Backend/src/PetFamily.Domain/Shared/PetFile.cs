using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public record PetFile
{
    public FilePath PathToStorage { get; }

    private PetFile(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public static Result<PetFile, Error> Create(FilePath pathToStorage)
    {
        return new PetFile(pathToStorage);
    }
}
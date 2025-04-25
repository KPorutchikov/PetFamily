using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public record FilePath
{
    public string Path { get; }
    
    private FilePath(string path)
    {
        Path = path;
    }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        // валидация разная 

        var fullPath = path + extension;
        
        return new FilePath(fullPath);
    }
    
    public static Result<FilePath, Error> Create(string fullPath)
    {
        return new FilePath(fullPath);
    }
}
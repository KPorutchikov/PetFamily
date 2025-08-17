using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Shared.Core.Shared;

public record FilePath
{
    public string Path { get; }

    private FilePath(string path)
    {
        Path = path;
    }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        var fullPath = path + extension;
        
        return new FilePath(fullPath);
    }
    
    public static Result<FilePath, Error> Create(string fullPath)
    {
        return new FilePath(fullPath);
    }
}
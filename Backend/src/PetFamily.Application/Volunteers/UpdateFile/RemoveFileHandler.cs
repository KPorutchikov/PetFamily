using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateFile;

public class RemoveFileHandler
{
    private readonly IFileProvider _fileProvider;

    public RemoveFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }
    
    public async Task<Result<string, Error>> Handle(string objectName, CancellationToken ct = default)
    {
        return await _fileProvider.RemoveFile(objectName, ct);
    }
}
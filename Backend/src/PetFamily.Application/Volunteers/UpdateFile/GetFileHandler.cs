using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateFile;

public class GetFileHandler
{
    private readonly IFileProvider _fileProvider;

    public GetFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }
    
    public async Task<Result<string, Error>> Handle(string objectName, CancellationToken ct = default)
    {
        return await _fileProvider.GetFileByObjectName(objectName, ct);
    }
}
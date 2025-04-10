using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateFile;

public class AddFileHandler
{
    private readonly IFileProvider _fileProvider;

    public AddFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }
    
    public async Task<Result<string, Error>> Handle(FileData fileData, CancellationToken ct = default)
    {
        return await _fileProvider.UploadFile(fileData, ct);
    }
}
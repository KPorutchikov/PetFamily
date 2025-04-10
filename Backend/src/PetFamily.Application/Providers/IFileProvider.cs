using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(FileData file, CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> RemoveFile(string objectName, CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> GetFileByObjectName(string objectName, CancellationToken cancellationToken = default);
}
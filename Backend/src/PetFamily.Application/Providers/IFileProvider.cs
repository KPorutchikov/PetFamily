using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData, 
        CancellationToken ct = default);

    Task<UnitResult<Error>> RemoveFile(
        FileInfo fileInfo, 
        CancellationToken ct = default);
}
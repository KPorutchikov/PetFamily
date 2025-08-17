using CSharpFunctionalExtensions;
using PetFamily.Shared.Core.FileProvider;
using PetFamily.Shared.Core.Shared;
using PetFamily.Shared.SharedKernel;
using FileInfo = PetFamily.Shared.Core.FileProvider.FileInfo;

namespace PetFamily.Shared.Core.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData, 
        CancellationToken ct = default);

    Task<UnitResult<Error>> RemoveFile(
        FileInfo fileInfo, 
        CancellationToken ct = default);
}
using PetFamily.Shared.Core.Shared;

namespace PetFamily.Shared.Core.FileProvider;

public record FileData(Stream Stream, FileInfo Info);

public record FileInfo(FilePath FilePath, string BucketName);
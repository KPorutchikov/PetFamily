using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileProvider;

public record FileData(Stream Stream, FileInfo Info);

public record FileInfo(FilePath FilePath, string BucketName);
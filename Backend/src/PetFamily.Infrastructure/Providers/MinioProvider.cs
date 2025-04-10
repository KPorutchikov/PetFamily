using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadFile(FileData file, CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket("photos");

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var bucketArgs = new MakeBucketArgs().WithBucket("photos");
                await _minioClient.MakeBucketAsync(bucketArgs, cancellationToken);
            }

            var path = Guid.NewGuid();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket("photos")
                .WithStreamData(file.Stream)
                .WithObjectSize(file.Stream.Length)
                .WithObject(path.ToString());

            var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return result.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error to upload file in minio");
            return Error.Failure("file.upload", "Error to upload file in minio");
        }
    }

    public async Task<Result<string, Error>> RemoveFile(string objectName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket("photos")
                .WithObject(objectName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            return objectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error to remove file in minio");
            return Error.Failure("file.delete", "Error to remove file in minio");
        }
    }

    public async Task<Result<string, Error>> GetFileByObjectName(string objectName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fileObjectArgs = new PresignedGetObjectArgs()
                .WithBucket("photos")
                .WithObject(objectName)
                .WithExpiry(60 * 60 * 24);

            var resultFile = await _minioClient.PresignedGetObjectAsync(fileObjectArgs);
            if (string.IsNullOrWhiteSpace(resultFile))
                return Error.NotFound("file.get", "File not found in minio");

            return resultFile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error to get file in minio");
            return Error.Failure("file.get", "Error to get file in minio");
        }
    }
}
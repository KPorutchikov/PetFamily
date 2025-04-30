using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddPetPhotos;

public class AddPetPhotosHandler
{
    private const string BUCKET_NAME = "files";
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetPhotosHandler(
        IFileProvider fileProvider,
        IVolunteerRepository volunteerRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(AddPetPhotosCommand command, CancellationToken ct = default)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var petResult = await _volunteerRepository.GetPetById(command.PetId, ct);
            if (petResult.IsFailure)
                return petResult.Error;

            List<FileData> filesData = [];
            foreach (var file in command.Files)
            {
                var extension = Path.GetExtension(file.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);
                if (filePath.IsFailure)
                    return filePath.Error;

                var fileContent = new FileData(file.Content, filePath.Value, BUCKET_NAME);

                filesData.Add(fileContent);
            }

            var petFiles = filesData
                .Select(f => f.FilePath)
                .Select(f => PetFile.Create(f).Value)
                .ToList();

            petResult.Value.UpdateFilesList(petFiles);

            await _unitOfWork.SaveChanges(ct);

            var uploadResult = await _fileProvider.UploadFiles(filesData, ct);

            if (uploadResult.IsFailure)
                return uploadResult.Error;

            transaction.Commit();

            return petResult.Value.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can not add files to pet - {id} in transaction", command.PetId);

            transaction.Rollback();

            return Error.Failure("Can not add files to pet", "volunteer.pet.failure");
        }
    }
}
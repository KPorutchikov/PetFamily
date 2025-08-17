using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.Core.FileProvider;
using PetFamily.Shared.Core.Messaging;
using PetFamily.Shared.Core.Providers;
using PetFamily.Shared.Core.Shared;
using PetFamily.Shared.SharedKernel;
using PetFamily.Volunteers.Application.Volunteers.AddPet;
using FileInfo = PetFamily.Shared.Core.FileProvider.FileInfo;

namespace PetFamily.Volunteers.Application.Volunteers.AddPetPhotos;

public class AddPetPhotosHandler : ICommandHandler<Guid,AddPetPhotosCommand>
{
    //private const string BUCKET_NAME = "files";
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddPetPhotosCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetPhotosHandler(
        IFileProvider fileProvider,
        IVolunteerRepository volunteerRepository,
        [FromKeyedServices(Modules.Volunteers)]IUnitOfWork unitOfWork,
        IValidator<AddPetPhotosCommand> validator,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        ILogger<AddPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _messageQueue = messageQueue;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddPetPhotosCommand command, CancellationToken ct = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, ct);
        if (validatorResult.IsValid == false)
            return validatorResult.ToErrorList();
        
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var petResult = await _volunteerRepository.GetPetById(command.PetId, ct);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            List<FileData> filesData = [];
            foreach (var file in command.Files)
            {
                var extension = Path.GetExtension(file.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);
                if (filePath.IsFailure)
                    return filePath.Error.ToErrorList();

                var fileContent = new FileData(file.Content, new FileInfo(filePath.Value, Constants.BUCKET_NAME));

                filesData.Add(fileContent);
            }

            var petFiles = filesData
                .Select(f => f.Info.FilePath)
                .Select(f => PetFile.Create(f).Value)
                .ToList();

            petResult.Value.UpdateFilesList(petFiles);

            await _unitOfWork.SaveChanges(ct);

            var uploadResult = await _fileProvider.UploadFiles(filesData, ct);
            if (uploadResult.IsFailure)
            {
                await _messageQueue.WriteAsync(filesData.Select(f => f.Info), ct);
                
                return uploadResult.Error.ToErrorList();
            }

            transaction.Commit();

            return petResult.Value.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can not add files to pet - {id} in transaction", command.PetId);

            transaction.Rollback();

            return Error.Failure("Can not add files to pet", "volunteer.pet.failure").ToErrorList();
        }
    }
}
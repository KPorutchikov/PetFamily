using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.Core.Providers;
using PetFamily.Shared.Core.Shared;
using PetFamily.Shared.SharedKernel;
using FileInfo = PetFamily.Shared.Core.FileProvider.FileInfo;

namespace PetFamily.Volunteers.Application.Volunteers.EditPet.DeletePet;

public class DeletePetHardHandler : ICommandHandler<Guid,DeletePetCommand>
{
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeletePetHardHandler> _logger;
    private readonly IValidator<DeletePetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;


    public DeletePetHardHandler(
        IFileProvider fileProvider,
        IVolunteerRepository volunteerRepository,
        ILogger<DeletePetHardHandler> logger, 
        IValidator<DeletePetCommand> validator,
        [FromKeyedServices(Modules.Volunteers)]IUnitOfWork unitOfWork)
    {
        _fileProvider = fileProvider;
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(DeletePetCommand command, CancellationToken ct)
    {
        var validationResult= await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var pet = await _volunteerRepository.GetPetById(command.PetId, ct);
            if (pet.IsFailure)
                return pet.Error.ToErrorList();
        
            var volunteer = await _volunteerRepository.GetByPetId(command.PetId, ct);
            if (volunteer.IsFailure)
                return volunteer.Error.ToErrorList();
            
            volunteer.Value.DeletePet(command.PetId);
            
            _volunteerRepository.HardDeletePet(pet.Value, ct);
            
            await _unitOfWork.SaveChanges(ct);

            foreach (var file in pet.Value.Files?.Values.ToList()! ?? Enumerable.Empty<PetFile>())
            {
                await _fileProvider.RemoveFile(new FileInfo(file.PathToStorage, Constants.BUCKET_NAME), ct);
            }

            _logger.LogInformation("Pet was deleted (hard) with id: {Id}.", command.PetId);
            
            transaction.Commit();

            return command.PetId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can not delete pet - {id} in transaction", command.PetId);

            transaction.Rollback();

            return Error.Failure("Can not delete pet", "volunteer.pet.failure").ToErrorList();
        }
    }
}
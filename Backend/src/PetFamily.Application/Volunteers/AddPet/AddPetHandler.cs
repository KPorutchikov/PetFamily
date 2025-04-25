using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetHandler
{
    private const string BUCKET_NAME = "files";
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetHandler(
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
    
    public async Task<Result<Guid, Error>> Handle(AddPetCommand command, CancellationToken ct = default)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var volunteerResult = await _volunteerRepository.GetById(VolunteerId.Create(command.VolunteerId), ct);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error;
        
            var breed = PetBreed.Create(command.SpeciesId, command.BreedId).Value;
            var address = Address.Create(command.City, command.Street, command.HouseNumber, command.ApartmentNumber).Value;
            var status = PetStatus.Create((PetStatus.Status)command.Status).Value;

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
            
            var pet = Pet.Create(PetId.NewId(), command.Name, breed, command.Description, command.Color,
                command.Height, command.Weight, command.HealthInformation, address, command.Phone,
                command.IsCastrated, command.BirthDate, command.IsVaccinated, status, 
                petFiles).Value;

            volunteerResult.Value.AddPet(pet);

            await _unitOfWork.SaveChanges(ct);
            
            var uploadResult = await _fileProvider.UploadFiles(filesData, ct);
            
            if (uploadResult.IsFailure)
                return uploadResult.Error;
            
            transaction.Commit();
            
            return pet.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Can not add pet to volunteer - {id} in transaction", command.VolunteerId);

            transaction.Rollback();

            return Error.Failure("Can not add pet to volunteer - {id}", "module.issue.failure");
        }
    }
}
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.AddPetPhotos;

public record CreateFileDto(Stream Content, string FileName);

public record AddPetPhotosCommand(Guid PetId, IEnumerable<CreateFileDto> Files) : ICommand;
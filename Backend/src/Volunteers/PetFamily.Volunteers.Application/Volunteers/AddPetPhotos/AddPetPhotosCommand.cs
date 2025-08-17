using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.AddPetPhotos;

public record CreateFileDto(Stream Content, string FileName);

public record AddPetPhotosCommand(Guid PetId, IEnumerable<CreateFileDto> Files) : ICommand;
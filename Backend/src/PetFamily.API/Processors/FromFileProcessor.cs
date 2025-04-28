using PetFamily.Application.Volunteers.AddPetPhotos;

namespace PetFamily.API.Processors;

public class FromFileProcessor : IAsyncDisposable
{
    private readonly List<CreateFileDto> _filesDtos = [];

    public List<CreateFileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileDto = new CreateFileDto(stream, file.FileName);
            _filesDtos.Add(fileDto);
        }
        
        return _filesDtos;
    }
    
    public async ValueTask DisposeAsync()
    {
        foreach (var file in _filesDtos)
        {
            await file.Content.DisposeAsync();
        }
    }
}
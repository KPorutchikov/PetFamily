using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.UpdateFile;

namespace PetFamily.API.Controllers;

public class FilesController : ApplicationController
{
    [HttpPost("Photo")]
    public async Task<ActionResult> AddFile(
        IFormFile file,
        [FromServices] AddFileHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        await using var stream = file.OpenReadStream();

        var fileData = new FileData(stream,"photos", Guid.NewGuid().ToString()); 

        var result = await handler.Handle(fileData, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpDelete("{objectName:guid}")]
    public async Task<ActionResult> RemoveFile(
        [FromRoute] Guid objectName,
        [FromServices] RemoveFileHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var result = await handler.Handle(objectName.ToString(), cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpGet("{objectName:guid}")]
    public async Task<ActionResult> GetFile(
        [FromRoute] Guid objectName,
        [FromServices] GetFileHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var result = await handler.Handle(objectName.ToString(), cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}
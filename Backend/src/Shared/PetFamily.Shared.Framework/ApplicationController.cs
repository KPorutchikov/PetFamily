using Microsoft.AspNetCore.Mvc;
using PetFamily.Shared.Core.Models;

namespace PetFamily.Shared.Framework;

[ApiController]
[Route("[controller]")]
public abstract class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);
        return base.Ok(envelope);
    }
}
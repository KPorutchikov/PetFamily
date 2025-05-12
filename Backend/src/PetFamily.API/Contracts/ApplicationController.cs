using Microsoft.AspNetCore.Mvc;

namespace PetFamily.API.Contracts;

[ApiController]
[Route("[controller]")]
public abstract class ApplicationController : ControllerBase
{
}
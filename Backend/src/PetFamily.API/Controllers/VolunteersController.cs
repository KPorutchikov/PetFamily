using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Controllers.Volunteers.Requests;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.VolunteerManagement.Queries.GetPet;
using PetFamily.Application.VolunteerManagement.Queries.GetPetsWithPagination;
using PetFamily.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPetPhotos;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.EditPet;
using PetFamily.Application.Volunteers.EditPet.DeletePet;
using PetFamily.Application.Volunteers.EditPet.UpdatePet;
using PetFamily.Application.Volunteers.EditPet.UpdatePetStatus;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdatePetMainPhoto;
using PetFamily.Application.Volunteers.UpdateRequisiteDetails;
using PetFamily.Application.Volunteers.UpdateSocialNetwork;

namespace PetFamily.API.Controllers;

public class VolunteersController : ApplicationController
{
    [HttpGet("{id:guid}/volunteer")]
    public async Task<ActionResult> VolunteerDapper(
        [FromRoute] Guid id,
        [FromServices] GetVolunteersWithPaginationHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVolunteersWithPaginationRequest(null, null, null, null, null, null);
        var volunteer = await handler.Handle(query.ToQuery(id), cancellationToken);

        return Ok(volunteer);
    }
    
    [HttpGet]
    public async Task<ActionResult> VolunteersWithPaginationDapper(
        [FromQuery] GetVolunteersWithPaginationRequest request,
        [FromServices] GetVolunteersWithPaginationHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var volunteers = await handler.Handle(request.ToQuery(null), cancellationToken);

        return Ok(volunteers);
    }
    
    [HttpGet("/pet")]
    public async Task<ActionResult> PetDapper(
        [FromQuery] GetPetRequest request,
        [FromServices] GetPetHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var pet = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(pet);
    }
    
    [HttpGet("/pets")]
    public async Task<ActionResult> PetsWithPaginationDapper(
        [FromQuery] GetPetsWithPaginationRequest request,
        [FromServices] GetPetsWithPaginationHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var pets = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(pets);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.Value;
    }

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPut("pet/{id:guid}/main_photo")]
    public async Task<ActionResult> UpdatePetMainPhoto(
        [FromRoute] Guid id,
        [FromBody] UpdatePetMainPhotoRequest request,
        [FromServices] UpdatePetMainPhotoHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("pet/{id:guid}/photos")]
    public async Task<IActionResult> AddPetPhotos(
        [FromRoute] Guid id,
        IFormFileCollection files,
        [FromServices] AddPetPhotosHandler handler,
        CancellationToken cancellationToken)
    {
        await using var fileProcessor = new FromFileProcessor();
        var fileDto = fileProcessor.Process(files);

        var command = new AddPetPhotosCommand(id, fileDto);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("pet/{id:guid}/serial_number")]
    public async Task<ActionResult> MovePet(
        [FromRoute] Guid id,
        [FromBody] MovePetRequest request,
        [FromServices] MovePetHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPut("pet/{id:guid}/main")]
    public async Task<ActionResult> UpdatePet(
        [FromRoute] Guid id,
        [FromServices] UpdatePetHandler handler,
        [FromBody] UpdatePetRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPut("pet/{id:guid}/status")]
    public async Task<ActionResult> UpdateStatusPet(
        [FromRoute] Guid id,
        [FromServices] UpdatePetStatusHandler handler,
        [FromBody] UpdatePetStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("pet/{id:guid}/soft")]
    public async Task<ActionResult> SoftDeletePet(
        [FromRoute] Guid id,
        [FromServices] DeletePetSoftHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var result = await handler.Handle(new DeletePetCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("pet/{id:guid}/hard")]
    public async Task<ActionResult> HardDeletePet(
        [FromRoute] Guid id,
        [FromServices] DeletePetHardHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var result = await handler.Handle(new DeletePetCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromServices] UpdateSocialNetworkHandler handler,
        [FromBody] UpdateSocialNetworkRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/requisite-details")]
    public async Task<ActionResult> UpdateRequisiteDetails(
        [FromRoute] Guid id,
        [FromServices] UpdateRequisiteDetailsHandler handler,
        [FromBody] UpdateRequisiteDetailsRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromServices] UpdateMainInfoHandler handler,
        [FromBody] UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}/soft")]
    public async Task<ActionResult> SoftDelete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerSoftHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = new DeleteVolunteerCommand(id);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult> HardDelete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHardHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = new DeleteVolunteerCommand(id);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
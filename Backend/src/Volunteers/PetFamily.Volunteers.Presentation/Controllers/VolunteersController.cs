using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Shared.Framework;
using PetFamily.Shared.Framework.Authorization;
using PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetPet;
using PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetPetsWithPagination;
using PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;
using PetFamily.Volunteers.Application.Volunteers.AddPet;
using PetFamily.Volunteers.Application.Volunteers.AddPetPhotos;
using PetFamily.Volunteers.Application.Volunteers.Create;
using PetFamily.Volunteers.Application.Volunteers.Delete;
using PetFamily.Volunteers.Application.Volunteers.EditPet;
using PetFamily.Volunteers.Application.Volunteers.EditPet.DeletePet;
using PetFamily.Volunteers.Application.Volunteers.EditPet.UpdatePet;
using PetFamily.Volunteers.Application.Volunteers.EditPet.UpdatePetStatus;
using PetFamily.Volunteers.Application.Volunteers.UpdateMainInfo;
using PetFamily.Volunteers.Application.Volunteers.UpdatePetMainPhoto;
using PetFamily.Volunteers.Application.Volunteers.UpdateRequisiteDetails;
using PetFamily.Volunteers.Application.Volunteers.UpdateSocialNetwork;
using PetFamily.Volunteers.Presentation.Pets.Requests;
using PetFamily.Volunteers.Presentation.Processors;
using PetFamily.Volunteers.Presentation.Volunteers.Requests;

namespace PetFamily.Volunteers.Presentation.Controllers;

[Authorize]
public class VolunteersController : ApplicationController
{
    [Permission(Permissions.Volunteer.VolunteerRead)]
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
    
    [Permission(Permissions.Volunteer.VolunteerRead)]
    [HttpGet]
    public async Task<ActionResult> VolunteersWithPaginationDapper(
        [FromQuery] GetVolunteersWithPaginationRequest request,
        [FromServices] GetVolunteersWithPaginationHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var volunteers = await handler.Handle(request.ToQuery(null), cancellationToken);

        return Ok(volunteers);
    }
    
    [Permission(Permissions.Volunteer.VolunteerRead)]
    [HttpGet("/pet")]
    public async Task<ActionResult> PetDapper(
        [FromQuery] GetPetRequest request,
        [FromServices] GetPetHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var pet = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(pet);
    }
    
    [Permission(Permissions.Volunteer.VolunteerRead)]
    [HttpGet("/pets")]
    public async Task<ActionResult> PetsWithPaginationDapper(
        [FromQuery] GetPetsWithPaginationRequest request,
        [FromServices] GetPetsWithPaginationHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var pets = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(pets);
    }

    [Permission(Permissions.Volunteer.VolunteerCreate)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.Value;
    }

    [Permission(Permissions.Volunteer.VolunteerCreate)]
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
    
    [Permission(Permissions.Volunteer.VolunteerUpdate)]
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

    [Permission(Permissions.Volunteer.VolunteerCreate)]
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

    [Permission(Permissions.Volunteer.VolunteerMove)]
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
    
    [Permission(Permissions.Volunteer.VolunteerUpdate)]
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
    
    [Permission(Permissions.Volunteer.VolunteerUpdate)]
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
    
    [Permission(Permissions.Volunteer.VolunteerDelete)]
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
    
    [Permission(Permissions.Volunteer.VolunteerDelete)]
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
    
    [Permission(Permissions.Volunteer.VolunteerUpdate)]
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

    [Permission(Permissions.Volunteer.VolunteerUpdate)]
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

    [Permission(Permissions.Volunteer.VolunteerUpdate)]
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

    [Permission(Permissions.Volunteer.VolunteerDelete)]
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

    [Permission(Permissions.Volunteer.VolunteerDelete)]
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
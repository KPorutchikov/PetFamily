using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPetPhotos;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.EditPet;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisiteDetails;
using PetFamily.Application.Volunteers.UpdateSocialNetwork;

namespace PetFamily.API.Controllers;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = new AddPetCommand(id, request.Name, request.SpeciesId, request.BreedId,
            request.Description, request.Color, request.Weight, request.Height, request.HealthInformation,
            request.City, request.Street, request.HouseNumber, request.ApartmentNumber, request.Phone,
            request.IsCastrated, request.BirthDate, request.IsVaccinated, request.Status);

        var result = await handler.Handle(command, cancellationToken);
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
        var command = new MovePetCommand(id, request.SerialNumber);
    
         var result = await handler.Handle(command, cancellationToken);
         if (result.IsFailure)
             return result.Error.ToResponse();
    
        return Ok();
    }

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromServices] UpdateSocialNetworkHandler handler,
        [FromBody] UpdateSocialNetworkRequest socialNetworks,
        [FromServices] IValidator<UpdateSocialNetworkRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateSocialNetworkRequest(id, socialNetworks.Dto);

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/requisite-details")]
    public async Task<ActionResult> UpdateRequisiteDetails(
        [FromRoute] Guid id,
        [FromServices] UpdateRequisiteDetailsHandler handler,
        [FromBody] UpdateRequisiteDetailsRequest requisiteDetails,
        [FromServices] IValidator<UpdateRequisiteDetailsRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateRequisiteDetailsRequest(id, requisiteDetails.Dto);

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromServices] UpdateMainInfoHandler handler,
        [FromBody] UpdateMainInfoDto dto,
        [FromServices] IValidator<UpdateMainInfoRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateMainInfoRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

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
        var request = new DeleteVolunteerRequest(id);

        var result = await handler.Handle(request, cancellationToken);
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
        var request = new DeleteVolunteerRequest(id);

        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Controllers.Species.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Species.DeleteBreed;
using PetFamily.Application.Species.DeleteSpecies;
using PetFamily.Application.SpeciesManagement.Queries.GetBreedsDapper;
using PetFamily.Application.SpeciesManagement.Queries.GetSpeciesDapper;

namespace PetFamily.API.Controllers;

public class SpeciesController : ApplicationController
{
    [HttpGet("dapper/species")]
    public async Task<ActionResult> SpeciesDapper(
        [FromServices] GetSpeciesHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var species = await handler.Handle(new GetSpeciesQuery(null), cancellationToken);
    
        return Ok(species);
    }

    [HttpPut("dapper/breed")]
    public async Task<ActionResult> BreedDapper(
        [FromBody] CreateBreedRequest request,
        [FromServices] GetBreedHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetBreedQuery(request.BreedId, request.SpeciesId);
        var breed = await handler.Handle(query, cancellationToken);

        return Ok(breed);
    }

    [HttpDelete("{id:guid}/species")]
    public async Task<ActionResult> DeleteSpecies(
        [FromRoute] Guid id,
        [FromServices] DeleteSpeciesHandler  handler,
        CancellationToken cancellationToken = default
    )
    {
        var result = await handler.Handle(new DeleteSpeciesCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}/breed")]
    public async Task<ActionResult> DeleteBreed(
        [FromRoute] Guid id,
        [FromServices] DeleteBreedHandler  handler,
        CancellationToken cancellationToken = default
    )
    {
        var result = await handler.Handle(new DeleteBreedCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
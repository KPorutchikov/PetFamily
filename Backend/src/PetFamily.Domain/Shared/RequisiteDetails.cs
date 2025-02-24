namespace PetFamily.Domain.Shared;

public record RequisiteDetails()
{
    public List<Requisites> RequisitesList { get; private set; }
}
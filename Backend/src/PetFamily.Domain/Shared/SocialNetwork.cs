using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public class SocialNetwork : ComparableValueObject
{
    public string Link { get; }
    public string Title { get; }
    private SocialNetwork(string link, string title)
    {
        Link = link;
        Title = title;
    }

    public static Result<SocialNetwork, Error> Create(string link, string title)
    {
        if (string.IsNullOrWhiteSpace(link))
            return Errors.General.ValueIsInvalid("link");
            
        if (string.IsNullOrWhiteSpace(title))
            return Errors.General.ValueIsInvalid("title");
        
        return new SocialNetwork(link, title);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Link;
        yield return Title;
    }
}
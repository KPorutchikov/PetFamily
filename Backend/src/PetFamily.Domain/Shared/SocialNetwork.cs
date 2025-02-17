using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteers;

public class SocialNetwork : ComparableValueObject
{
    public string Link { get; private set; }
    public string Title { get; private set; }

    // for EF Core
    private SocialNetwork() { }
    
    private SocialNetwork(string link, string title)
    {
        Link = link;
        Title = title;
    }

    public static Result<SocialNetwork> Create(string link, string title)
    {
        if (string.IsNullOrWhiteSpace(link))
            return Result.Failure<SocialNetwork>($"{nameof(link)} is not be empty");
            
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<SocialNetwork>($"{nameof(title)} is not be empty");

        var socialNetwork = new SocialNetwork(link, title);
        
        return Result.Success(socialNetwork);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Link;
        yield return Title;
    }
}
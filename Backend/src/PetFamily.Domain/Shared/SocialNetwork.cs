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

    public static Result<SocialNetwork> Create(string link, string title)
    {
        if (string.IsNullOrWhiteSpace(link))
            return Result.Failure<SocialNetwork>($"{nameof(link)} is not be empty");
            
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<SocialNetwork>($"{nameof(title)} is not be empty");
        
        return Result.Success(new SocialNetwork(link, title));
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Link;
        yield return Title;
    }
}
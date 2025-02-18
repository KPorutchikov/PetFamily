﻿using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public record SocialNetwork
{
    public string Link { get; private set; }
    public string Title { get; private set; }
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
}
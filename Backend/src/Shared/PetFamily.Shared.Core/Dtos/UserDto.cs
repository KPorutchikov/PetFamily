using PetFamily.Shared.SharedKernel;

namespace PetFamily.Shared.Core.Dtos;

public class UserDto
{
    public Guid Id { get; init; }
    
    public string UserName { get; init; } = string.Empty;
    
    public string Email { get; init; } = string.Empty;
    
    public string PhoneNumber { get; init; } = string.Empty;
    
    public UserAccountDto[] Accounts { get; set; } = null!;

}

public class UserAccountDto
{
    public Guid IdAccount { get; init; }
    
    public UserAccountType AccountType { get; init; }
}


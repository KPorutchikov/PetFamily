using System.Data;

namespace PetFamily.Shared.Core.Abstractions;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}
using System.Reflection;

namespace Litres.Data.Exceptions;

public class EntityNotFoundException(MemberInfo entity, string? key) 
    : Exception($"{entity.Name} {key} was not found")
{
}
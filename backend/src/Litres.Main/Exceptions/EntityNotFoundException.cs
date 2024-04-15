using System.Reflection;
using Litres.Data.Models;

namespace Litres.Main.Exceptions;

public class EntityNotFoundException(MemberInfo entity, string? key) 
    : Exception($"{entity.Name} {key} was not found")
{
}
using System.Reflection;

namespace Litres.Main.Exceptions;

public class EntityUnprocessableException(MemberInfo entityType, string entityId, string reason) 
    : Exception($"{entityType.Name} with {entityId} can't be handled in current context: {reason}");
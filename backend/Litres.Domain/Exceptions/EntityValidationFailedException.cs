using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Litres.Domain.Exceptions;

public class EntityValidationFailedException(MemberInfo entity, IEnumerable<ValidationResult> results) : Exception(
    $"{entity.Name} is invalid. Info: {string.Join("; ", results.Select(result => result.ErrorMessage))}"
);

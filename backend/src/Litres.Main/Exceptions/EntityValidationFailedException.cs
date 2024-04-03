using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Litres.Main.Exceptions;

public class EntityValidationFailedException(MemberInfo entity, IEnumerable<ValidationResult> results) : Exception(
    $"{entity.Name} is invalid. Info: {string.Join("; ", results.Select(result => result.ErrorMessage))}"
);

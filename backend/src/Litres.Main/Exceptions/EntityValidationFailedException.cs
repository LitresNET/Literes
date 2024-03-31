using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Litres.Main.Exceptions;

public class EntityValidationFailedException<TEntity>(IEnumerable<ValidationResult> results) : Exception(
    $"{typeof(TEntity)} is invalid. Info: {string.Join("; ", results.Select(result => result.ErrorMessage))}"
);

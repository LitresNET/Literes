using System.ComponentModel.DataAnnotations;

namespace Litres.Main.Exceptions;

public class BookValidationFailedException(List<ValidationResult> results)
    : Exception(
        $"Book is invalid. Info: {string.Join("; ", results.Select(result => result.ErrorMessage))}"
    );

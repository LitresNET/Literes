using System.ComponentModel.DataAnnotations;

namespace backend.Exceptions;

public class BookValidationFailedException(List<ValidationResult> results)
    : Exception(
        $"Book is invalid. Info: {string.Join("; ", results.Select(result => result.ErrorMessage))}"
    );

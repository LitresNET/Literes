namespace Litres.Domain.Exceptions;

public class MetadataUploadError(string filename, string message) : 
    Exception($"Error while uploading metadata for {filename}: {message}");
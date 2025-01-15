using Microsoft.AspNetCore.Http;
using File = TagLib.File;

namespace Litres.Application.Models;

public class StreamFileAbstraction(IFormFile formFile) : File.IFileAbstraction
{
    private readonly Stream _stream = formFile.OpenReadStream();
    private readonly string _name = formFile.FileName;

    public string Name => _name;

    public Stream ReadStream => _stream;

    public Stream WriteStream => _stream;
    public void CloseStream(Stream stream)
    {
        if (stream == _stream)
        {
            _stream.Dispose();
        }
    }
}   
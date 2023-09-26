using Microsoft.AspNetCore.Http;
using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.Images;

public class UploadImageCommand : ICommand<string>
{
    public IFormFile File { get; set; }

    public UploadImageCommand(IFormFile file)
    {
        File = file;
    }
}